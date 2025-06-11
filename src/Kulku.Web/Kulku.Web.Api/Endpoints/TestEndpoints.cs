using Kulku.Domain.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Kulku.Web.Api.Endpoints;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("test");

        group.MapGet("test", GetLocalizedTest).WithName(nameof(GetLocalizedTest));
    }

    public static Results<Ok<string>, BadRequest<ProblemDetails>> GetLocalizedTest()
    {
        return TypedResults.Ok(Strings.Test);
    }
}
