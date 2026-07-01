using Microsoft.AspNetCore.Http;

namespace Kulku.Web.AspNetCore.Http;

/// <summary>
/// Identifies request paths that generate high-frequency, low-value log events and trace spans.
/// Used by both Serilog request logging and the OpenTelemetry ASP.NET Core instrumentation filter.
/// </summary>
internal static class NoisyPaths
{
    /// <summary>
    /// Returns <see langword="true"/> when the path belongs to a health check, static asset,
    /// or framework endpoint that should not be logged or traced at normal verbosity.
    /// </summary>
    internal static bool IsNoisyPath(PathString path)
    {
        return path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase)
            || path.StartsWithSegments("/openapi", StringComparison.OrdinalIgnoreCase)
            || path.StartsWithSegments("/_framework", StringComparison.OrdinalIgnoreCase)
            || path.StartsWithSegments("/_content", StringComparison.OrdinalIgnoreCase)
            || path.StartsWithSegments("/css", StringComparison.OrdinalIgnoreCase)
            || path.StartsWithSegments("/js", StringComparison.OrdinalIgnoreCase)
            || path.StartsWithSegments("/images", StringComparison.OrdinalIgnoreCase)
            || path.Equals("/favicon.ico", StringComparison.OrdinalIgnoreCase);
    }
}
