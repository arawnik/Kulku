using Carter;
using Kulku.Domain.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Kulku.Web.Api.Endpoints;

public class TestEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("test");

        group.MapGet("test", GetLocalizedTest).WithName(nameof(GetLocalizedTest));
    }

    public static Results<Ok<string>, BadRequest<ProblemDetails>> GetLocalizedTest()
    {
        return TypedResults.Ok(Strings.Test);
    }
}
