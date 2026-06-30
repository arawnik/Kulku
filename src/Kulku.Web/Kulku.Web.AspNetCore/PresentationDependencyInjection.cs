using System.Diagnostics.CodeAnalysis;
using Kulku.Web.AspNetCore.Logging;
using Kulku.Web.AspNetCore.Observability;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            .Validate(
                o =>
                    !o.TraceSampleRatio.HasValue
                    || (o.TraceSampleRatio.Value >= 0.0 && o.TraceSampleRatio.Value <= 1.0),
                "Observability:TraceSampleRatio must be between 0.0 and 1.0."
            );

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
            );

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
                    .AddAspNetCoreInstrumentation(o => o.RecordException = true)
                    .AddHttpClientInstrumentation()
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

        if (ratio is < 0.0 or > 1.0)
        {
            throw new InvalidOperationException(
                "Observability:TraceSampleRatio must be between 0.0 and 1.0."
            );
        }

        tracing.SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(ratio)));
    }
}
