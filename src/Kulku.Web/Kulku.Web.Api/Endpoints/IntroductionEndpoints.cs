using Carter;
using Kulku.Application.Cover;
using Kulku.Contract.Cover;
using Kulku.Web.Api.Middleware;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Api.Endpoints;

public class IntroductionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("introduction");

        group.MapGet("", GetIntroductionAsync).WithName(nameof(GetIntroductionAsync));
    }

    public static async Task<
        Results<Ok<IntroductionResponse>, NotFound, BadRequest<ProblemDetails>>
    > GetIntroductionAsync(
        [FromServices] IQueryHandler<GetIntroduction.Query, IntroductionResponse?> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(new GetIntroduction.Query(), cancellationToken);
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return result.HandleFailure();
    }
}
