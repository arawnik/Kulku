using Carter;
using Kulku.Application.Cover;
using Kulku.Contract.Cover;
using Kulku.Web.Api.Middleware;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Api.Endpoints;

public class ExperienceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("experience");

        group.MapGet("", GetExperiencesAsync).WithName(nameof(GetExperiencesAsync));
    }

    public static async Task<
        Results<Ok<ICollection<ExperienceResponse>>, NotFound, BadRequest<ProblemDetails>>
    > GetExperiencesAsync(
        [FromServices] IQueryHandler<GetExperiences.Query, ICollection<ExperienceResponse>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(new GetExperiences.Query(), cancellationToken);
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return result.HandleFailure();
    }
}
