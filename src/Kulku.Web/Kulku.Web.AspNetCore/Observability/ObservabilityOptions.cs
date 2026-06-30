namespace Kulku.Web.AspNetCore.Observability;

/// <summary>
/// Options for OpenTelemetry observability (traces, metrics, and logs).
/// Bound from the "Observability" configuration section.
/// </summary>
public sealed class ObservabilityOptions
{
    public const string SectionName = "Observability";

    /// <summary>
    /// Service name reported in all signals (traces, metrics, logs).
    /// Used to identify this app in Grafana dashboards alongside other services.
    /// Defaults to "Kulku".
    /// </summary>
    public string? ServiceName { get; set; } = "Kulku";

    /// <summary>
    /// Service version that will be reported in all signals (traces, metrics, logs).
    /// </summary>
    public string? ServiceVersion { get; set; }

    /// <summary>
    /// OTLP exporter endpoint (e.g. "http://otel:4318").
    /// When null or empty, OTLP export is disabled and no telemetry is sent.
    /// Do not include /v1/traces, /v1/metrics, or /v1/logs here.
    /// </summary>
    public string? OtlpEndpoint { get; set; }

    /// <summary>
    /// OTLP transport protocol. Valid values: "grpc", "http/protobuf".
    /// Supported values: http/protobuf, grpc.
    /// </summary>
    public string OtlpProtocol { get; set; } = "http/protobuf";

    /// <summary>
    /// Optional OTLP headers.
    /// Grafana Cloud example:
    /// Authorization=Basic base64(instanceId:token)
    /// </summary>
    public string? OtlpHeaders { get; set; }

    /// <summary>
    /// Enable when traces should be exported through OTLP.
    /// </summary>
    public bool TracesEnabled { get; set; } = true;

    /// <summary>
    /// Enable when metrics should be exported through OTLP.
    /// </summary>
    public bool MetricsEnabled { get; set; } = true;

    /// <summary>
    /// Enable when ILogger logs should be exported through OTLP via the Serilog OpenTelemetry sink.
    /// <para>False by default because of Serilog file logs.</para>
    /// </summary>
    public bool LogsEnabled { get; set; } // = false; (default)

    /// <summary>
    /// Optional. When null, the OpenTelemetry .NET SDK default sampler is used.
    /// Configure only when volume/cost/noise becomes a problem.
    /// </summary>
    public double? TraceSampleRatio { get; set; }
}
