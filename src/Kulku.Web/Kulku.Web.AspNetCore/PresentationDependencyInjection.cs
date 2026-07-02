using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Kulku.Web.AspNetCore.Health;
using Kulku.Web.AspNetCore.Http;
using Kulku.Web.AspNetCore.Logging;
using Kulku.Web.AspNetCore.Observability;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Kulku.Web.AspNetCore;

[ExcludeFromCodeCoverage]
public static class PresentationDependencyInjection
{
    /// <summary>
    /// Binds presentation-specific options from configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register options into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddPresentationOptions(this IServiceCollection services)
    {
        services
            .AddOptions<ObservabilityOptions>()
            .BindConfiguration(ObservabilityOptions.SectionName)
            // Guard #1: validates the DI-bound IOptions<ObservabilityOptions> instance at host start.
            // Guard #2 lives in ApplySamplerIfConfigured, which reads IConfiguration directly at
            // service-registration time (before Build/host start). Both guards are intentional
            // because they operate on independent object instances at different lifecycle stages.
            .Validate(
                o =>
                    !o.TraceSampleRatio.HasValue
                    || (o.TraceSampleRatio.Value >= 0.0 && o.TraceSampleRatio.Value <= 1.0),
                "Observability:TraceSampleRatio must be between 0.0 and 1.0."
            )
            .ValidateOnStart();

        services
            .AddOptions<KulkuLoggingOptions>()
            .BindConfiguration(KulkuLoggingOptions.SectionName)
            .Validate(
                o => o.RetainedFileCountLimit > 0,
                "Logging:RetainedFileCountLimit must be greater than zero."
            )
            .Validate(
                o => o.FileSizeLimitBytes > 0,
                "Logging:FileSizeLimitBytes must be greater than zero."
            )
            .ValidateOnStart();

        return services;
    }

    /// <summary>
    /// Registers core presentation services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddPresentationCore(this IServiceCollection services)
    {
        services
            .Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor
                    | ForwardedHeaders.XForwardedProto
                    | ForwardedHeaders.XForwardedHost;

                // Clear defaults so any reverse proxy (NPM, nginx) on a non-loopback network is trusted.
                // Safe as long as the app port is not publicly exposed.
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            })
            .AddLocalization()
            .AddHttpContextAccessor();

        return services;
    }

    /// <summary>
    /// Registers OpenTelemetry for traces, metrics, and logs.
    /// Uses one shared OTLP endpoint/protocol/header configuration,
    /// while allowing each telemetry signal to be enabled or disabled independently.
    /// </summary>
    public static IServiceCollection AddObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        var options =
            configuration.GetSection(ObservabilityOptions.SectionName).Get<ObservabilityOptions>()
            ?? new ObservabilityOptions();

        // Stop early if nothing is enabled
        // Note: LogsEnabled is handled by SerilogConfigurationExtensions (Serilog OTel sink),
        // not through the OpenTelemetry SDK provider, so it is not considered here.
        var anySignalEnabled = options.TracesEnabled || options.MetricsEnabled;
        if (!anySignalEnabled)
        {
            return services;
        }

        if (string.IsNullOrWhiteSpace(options.OtlpEndpoint))
        {
            if (environment.IsProduction())
            {
                throw new InvalidOperationException(
                    "OpenTelemetry export is enabled, but Observability:OtlpEndpoint is not configured."
                );
            }

            // Local dev, CI, QA, and other non-production environments can run without OTLP.
            return services;
        }

        var serviceName = string.IsNullOrWhiteSpace(options.ServiceName)
            ? "kulku"
            : options.ServiceName;
        var protocol = options.OtlpProtocol.ParseOtlpExportProtocol();

        var otelBuilder = services
            .AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource
                    .AddService(
                        serviceName: serviceName,
                        serviceVersion: options.ServiceVersion,
                        serviceInstanceId: Environment.MachineName
                    )
                    .AddAttributes(
                        [
                            new KeyValuePair<string, object>(
                                "deployment.environment.name",
                                environment.EnvironmentName
                            ),
                        ]
                    );
            });

        if (options.TracesEnabled)
        {
            otelBuilder.WithTracing(tracing =>
            {
                ApplySamplerIfConfigured(tracing, options);

                tracing
                    .AddAspNetCoreInstrumentation(o =>
                    {
                        o.RecordException = true;
                        // Exclude health probes and static assets — they generate high-frequency
                        // spans with no diagnostic value and inflate trace volume.
                        o.Filter = ctx => !NoisyPaths.IsNoisyPath(ctx.Request.Path);
                    })
                    .AddHttpClientInstrumentation()
                    // db.statement (SQL text) is intentionally not captured for security.
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql();

                tracing.AddOtlpExporter(exporter =>
                {
                    ConfigureOtlpExporter(exporter, options, protocol, OtlpSignalType.Traces);
                });
            });
        }

        if (options.MetricsEnabled)
        {
            otelBuilder.WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddNpgsqlInstrumentation()
                    .AddMeter("Microsoft.EntityFrameworkCore")
                    .AddMeter("Kulku.*")
                    .AddOtlpExporter(exporter =>
                    {
                        ConfigureOtlpExporter(exporter, options, protocol, OtlpSignalType.Metrics);
                    });
            });
        }

        return services;
    }

    private static void ConfigureOtlpExporter(
        OtlpExporterOptions exporter,
        ObservabilityOptions options,
        OtlpExportProtocol protocol,
        OtlpSignalType signalType
    )
    {
        exporter.Protocol = protocol;
        exporter.Endpoint = signalType.CreateOtlpSignalEndpoint(options.OtlpEndpoint, protocol);

        if (!string.IsNullOrWhiteSpace(options.OtlpHeaders))
        {
            exporter.Headers = options.OtlpHeaders.Trim();
        }
    }

    private static void ApplySamplerIfConfigured(
        TracerProviderBuilder tracing,
        ObservabilityOptions options
    )
    {
        if (!options.TraceSampleRatio.HasValue)
        {
            return;
        }

        var ratio = options.TraceSampleRatio.Value;

        // Guard #2: validates the IConfiguration-read options instance at service-registration time
        // (before Build/host start). Guard #1 lives in AddPresentationOptions via ValidateOnStart()
        // and operates on the DI-bound IOptions<ObservabilityOptions> instance at a later stage.
        // Both guards are intentional because they cover independent object instances.
        if (ratio is < 0.0 or > 1.0)
        {
            throw new InvalidOperationException(
                "Observability:TraceSampleRatio must be between 0.0 and 1.0."
            );
        }

        tracing.SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(ratio)));
    }

    /// <summary>
    /// Maps <c>/health/live</c> and <c>/health/ready</c> health check endpoints.
    /// <para>
    /// Use this in applications that do not use Carter (e.g. the Admin Blazor app).
    /// Applications using Carter should register a <c>HealthEndpoints : ICarterModule</c> instead.
    /// </para>
    /// </summary>
    public static IEndpointRouteBuilder MapPresentationHealthEndpoints(
        this IEndpointRouteBuilder app
    )
    {
        app.MapHealthChecks(
                "/health/live",
                new HealthCheckOptions
                {
                    Predicate = _ => false, // no checks: if the process responds, it's alive
                }
            )
            .AllowAnonymous();
        app.MapHealthChecks(
                "/health/ready",
                new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(HealthCheckTags.Ready),
                    ResponseWriter = WriteHealthJsonResponse,
                }
            )
            .AllowAnonymous();

        return app;
    }

    private static Task WriteHealthJsonResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.ToString(),
                description = e.Value.Exception?.Message,
            }),
        };

        return context.Response.WriteAsJsonAsync(
            result,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            }
        );
    }
}
