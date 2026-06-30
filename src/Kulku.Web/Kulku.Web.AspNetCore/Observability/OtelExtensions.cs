using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Exporter;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace Kulku.Web.AspNetCore.Observability;

internal static class OtelExtensions
{
    internal static OtlpExportProtocol ParseOtlpExportProtocol(this string? value)
    {
        return string.Equals(value?.Trim(), "grpc", StringComparison.OrdinalIgnoreCase)
            ? OtlpExportProtocol.Grpc
            : OtlpExportProtocol.HttpProtobuf;
    }

    /// <summary>
    /// Gets the expected OTLP endpoint path for the given signal type, based on the OTLP specification.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    internal static string GetOtlpSignalPath(this OtlpSignalType signalType) =>
        signalType switch
        {
            OtlpSignalType.Traces => "/v1/traces",
            OtlpSignalType.Metrics => "/v1/metrics",
            OtlpSignalType.Logs => "/v1/logs",
            _ => throw new InvalidOperationException($"Unsupported OTLP signal type: {signalType}"),
        };

    internal static Uri CreateOtlpSignalEndpoint(
        this OtlpSignalType signalType,
        string? endpoint,
        OtlpExportProtocol protocol
    )
    {
        var endpointBase = endpoint?.Trim() ?? string.Empty;

        // For Grpc just use the given endpoint.
        if (protocol == OtlpExportProtocol.Grpc)
        {
            return new Uri(endpointBase, UriKind.Absolute);
        }

        // For HttpProtobuf signal-specific exporters, provide the full signal endpoint.
        endpointBase = endpointBase.TrimEnd('/');
        var expectedSignalPath = signalType.GetOtlpSignalPath();

        return new Uri($"{endpointBase}{expectedSignalPath}", UriKind.Absolute);
    }

    internal static void ConfigureSerilogOtel(
        this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        // When OTLP log export is enabled, forward Serilog events to the collector via the
        // Serilog OpenTelemetry sink. This is required because AddSerilog() replaces the
        // entire .NET ILoggerFactory, so the OpenTelemetry SDK's ILoggerProvider never
        // receives log events.
        var observabilityOptions =
            configuration.GetSection(ObservabilityOptions.SectionName).Get<ObservabilityOptions>()
            ?? new ObservabilityOptions();

        if (observabilityOptions.LogsEnabled)
        {
            if (string.IsNullOrWhiteSpace(observabilityOptions.OtlpEndpoint))
            {
                if (environment.IsProduction())
                {
                    throw new InvalidOperationException(
                        "Observability:LogsEnabled is true, but Observability:OtlpEndpoint is not configured."
                    );
                }

                // Non-production environments can run without OTLP (local dev, CI, etc.).
                return;
            }

            var serviceName = string.IsNullOrWhiteSpace(observabilityOptions.ServiceName)
                ? "holdion"
                : observabilityOptions.ServiceName;
            var protocol = observabilityOptions.OtlpProtocol.ParseOtlpExportProtocol();
            var endpoint = OtlpSignalType.Logs.CreateOtlpSignalEndpoint(
                observabilityOptions.OtlpEndpoint,
                protocol
            );

            loggerConfiguration.WriteTo.OpenTelemetry(options =>
            {
                options.Protocol = ToSerilogOtlpProtocol(protocol);
                options.Endpoint = endpoint.AbsoluteUri;

                if (!string.IsNullOrWhiteSpace(observabilityOptions.OtlpHeaders))
                {
                    options.Headers = ParseOtlpHeaders(observabilityOptions.OtlpHeaders);
                }

                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = serviceName,
                    ["service.instance.id"] = Environment.MachineName,
                    ["deployment.environment.name"] = environment.EnvironmentName,
                };

                if (!string.IsNullOrWhiteSpace(observabilityOptions.ServiceVersion))
                {
                    options.ResourceAttributes["service.version"] =
                        observabilityOptions.ServiceVersion;
                }
            });
        }
    }

    /// <summary>
    /// Parses the OTLP header string (e.g. <c>Key1=Value1,Key2=Value2</c>) into a dictionary.
    /// This matches the <c>OTEL_EXPORTER_OTLP_HEADERS</c> format accepted by the OpenTelemetry SDK.
    /// </summary>
    private static IDictionary<string, string> ParseOtlpHeaders(string otlpHeaders)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (
            var part in otlpHeaders.Split(
                ',',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            )
        )
        {
            var separatorIndex = part.IndexOf('=', StringComparison.Ordinal);
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = part[..separatorIndex].Trim();
            var value = part[(separatorIndex + 1)..].Trim();

            if (!string.IsNullOrEmpty(key))
            {
                result[key] = value;
            }
        }

        return result;
    }

    private static OtlpProtocol ToSerilogOtlpProtocol(OtlpExportProtocol protocol)
    {
        return protocol switch
        {
            OtlpExportProtocol.Grpc => OtlpProtocol.Grpc,
            OtlpExportProtocol.HttpProtobuf => OtlpProtocol.HttpProtobuf,
            _ => throw new InvalidOperationException($"Unsupported OTLP protocol: {protocol}"),
        };
    }
}
