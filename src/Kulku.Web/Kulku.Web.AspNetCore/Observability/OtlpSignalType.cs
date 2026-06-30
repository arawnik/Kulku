namespace Kulku.Web.AspNetCore.Observability;

/// <summary>
/// Type of signal.
/// </summary>
internal enum OtlpSignalType
{
    Traces,
    Metrics,
    Logs,
}
