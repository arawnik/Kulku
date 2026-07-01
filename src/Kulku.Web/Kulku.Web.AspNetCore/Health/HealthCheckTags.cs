namespace Kulku.Web.AspNetCore.Health;

/// <summary>
/// Tag constants used to categorize health checks for liveness and readiness probes.
/// </summary>
public static class HealthCheckTags
{
    /// <summary>
    /// Tags a health check as a readiness dependency.
    /// Readiness checks are run by the <c>/health/ready</c> endpoint.
    /// </summary>
    public const string Ready = "ready";
}

/// <summary>
/// Name constants for registered health checks.
/// Used when registering checks and referenced in readiness probe responses.
/// </summary>
public static class HealthCheckNames
{
    /// <summary>
    /// Name of the main application database health check.
    /// </summary>
    public const string PostgresApp = "postgres-app";

    /// <summary>
    /// Name of the user/identity database health check.
    /// </summary>
    public const string PostgresUser = "postgres-user";
}
