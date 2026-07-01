using System.Globalization;
using Kulku.Web.AspNetCore.Http;
using Kulku.Web.AspNetCore.Observability;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Kulku.Web.AspNetCore.Logging;

public static class SerilogExtensions
{
    private static readonly IReadOnlyDictionary<string, LogEventLevel> DefaultOverrides =
        new Dictionary<string, LogEventLevel>(StringComparer.OrdinalIgnoreCase)
        {
            ["Microsoft"] = LogEventLevel.Warning,
            ["Microsoft.AspNetCore"] = LogEventLevel.Warning,
            ["Microsoft.Hosting.Lifetime"] = LogEventLevel.Information,
            ["Microsoft.EntityFrameworkCore.Database.Command"] = LogEventLevel.Warning,
            ["OpenTelemetry"] = LogEventLevel.Warning,
            ["System"] = LogEventLevel.Warning,
            ["System.Net.Http.HttpClient"] = LogEventLevel.Warning,
            ["Kulku"] = LogEventLevel.Information,
        };

    public static ILogger CreateBootstrapLogger(string applicationName)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            .CreateBootstrapLogger();
    }

    public static void AddPresentationSerilog(
        this WebApplicationBuilder builder,
        string applicationName
    )
    {
        builder.Services.AddSerilog(
            (services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ConfigureKulkuSerilog(
                        builder.Configuration,
                        builder.Environment,
                        applicationName
                    )
                    .ConfigureSerilogOtel(
                        builder.Configuration,
                        builder.Environment,
                        applicationName
                    );
            }
        );
    }

    public static IApplicationBuilder UsePresentationRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} => {StatusCode} in {Elapsed:0.0000} ms ({Protocol})";

            options.GetLevel = static (httpContext, _, exception) =>
            {
                if (exception is not null)
                    return LogEventLevel.Error;

                var statusCode = httpContext.Response.StatusCode;

                if (statusCode >= StatusCodes.Status500InternalServerError)
                    return LogEventLevel.Error;

                if (statusCode >= StatusCodes.Status400BadRequest)
                    return LogEventLevel.Warning;

                if (NoisyPaths.IsNoisyPath(httpContext.Request.Path))
                    return LogEventLevel.Debug;

                return LogEventLevel.Information;
            };

            options.EnrichDiagnosticContext = static (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("Protocol", httpContext.Request.Protocol);

                var userAgent = httpContext.Request.Headers.UserAgent.ToString();
                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    diagnosticContext.Set("UserAgent", userAgent);
                }

                if (httpContext.User.Identity?.IsAuthenticated == true)
                {
                    diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
                }
            };
        });

        return app;
    }

    private static LoggerConfiguration ConfigureKulkuSerilog(
        this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration,
        IHostEnvironment environment,
        string applicationName
    )
    {
        // Prevent accidental use of the full Serilog.Settings.Configuration DSL.
        // Sinks, enrichers, and formatters are intentionally owned by code here.
        ValidateNoUnsupportedSerilogConfiguration(configuration);

        var loggingOptions =
            configuration.GetSection(KulkuLoggingOptions.SectionName).Get<KulkuLoggingOptions>()
            ?? new KulkuLoggingOptions();

        var serilogOptions =
            configuration.GetSection(KulkuSerilogOptions.SectionName).Get<KulkuSerilogOptions>()
            ?? new KulkuSerilogOptions();

        var defaultLevel = ParseLogEventLevel(
            serilogOptions.MinimumLevel.Default,
            environment.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information,
            "Serilog:MinimumLevel:Default"
        );

        loggerConfiguration
            .MinimumLevel.Is(defaultLevel)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            .Enrich.WithProperty("Environment", environment.EnvironmentName);

        foreach (var item in DefaultOverrides)
        {
            loggerConfiguration.MinimumLevel.Override(item.Key, item.Value);
        }

        foreach (var item in serilogOptions.MinimumLevel.Override)
        {
            var level = ParseLogEventLevel(
                item.Value,
                fallback: null,
                settingName: $"Serilog:MinimumLevel:Override:{item.Key}"
            );

            loggerConfiguration.MinimumLevel.Override(item.Key, level);
        }

        if (loggingOptions.WriteToConsole || environment.IsDevelopment())
        {
            loggerConfiguration.WriteTo.Console(formatProvider: CultureInfo.InvariantCulture);
        }

        if (loggingOptions.WriteToFile)
        {
            Directory.CreateDirectory(loggingOptions.LogDirectory);

            var filePath = Path.Combine(
                loggingOptions.LogDirectory,
                $"{loggingOptions.FileNamePrefix}-.clef"
            );

            loggerConfiguration.WriteTo.Async(asyncSink =>
            {
                asyncSink.File(
                    formatter: new CompactJsonFormatter(),
                    path: filePath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: loggingOptions.RetainedFileCountLimit,
                    fileSizeLimitBytes: loggingOptions.FileSizeLimitBytes,
                    rollOnFileSizeLimit: true,
                    shared: true
                );
            });
        }

        return loggerConfiguration;
    }

    private static LogEventLevel ParseLogEventLevel(
        string? value,
        LogEventLevel? fallback,
        string settingName
    )
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            if (fallback.HasValue)
            {
                return fallback.Value;
            }

            throw new InvalidOperationException($"{settingName} must be configured.");
        }

        if (Enum.TryParse<LogEventLevel>(value, ignoreCase: true, out var level))
        {
            return level;
        }

        throw new InvalidOperationException(
            $"{settingName} has unsupported value '{value}'. "
                + "Use Verbose, Debug, Information, Warning, Error, or Fatal."
        );
    }

    private static void ValidateNoUnsupportedSerilogConfiguration(IConfiguration configuration)
    {
        var serilogSection = configuration.GetSection(KulkuSerilogOptions.SectionName);
        if (!serilogSection.Exists())
        {
            return;
        }

        var allowedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            nameof(KulkuSerilogOptions.MinimumLevel),
        };

        var unsupportedKeys = serilogSection
            .GetChildren()
            .Select(section => section.Key)
            .Where(key => !allowedKeys.Contains(key))
            .ToArray();
        if (unsupportedKeys.Length == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            "Only selected Serilog configuration sections are supported. "
                + "Sinks, enrichers, and formatters are configured in code. "
                + $"Unsupported Serilog sections: {string.Join(", ", unsupportedKeys)}."
        );
    }
}
