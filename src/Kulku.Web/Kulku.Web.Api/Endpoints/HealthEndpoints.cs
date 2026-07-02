using Carter;
using Kulku.Web.AspNetCore;

namespace Kulku.Web.Api.Endpoints;

/// <summary>
/// Maps liveness and readiness health check probes.
/// Both endpoints are anonymous — Kubernetes/Docker probes do not send credentials.
/// </summary>
public class HealthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPresentationHealthEndpoints();
    }
}
