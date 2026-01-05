using Carter;
using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover;
using Kulku.Application.Cover.Models;
using Kulku.Web.Api.Http;
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
        Results<Ok<IReadOnlyList<ExperienceModel>>, NotFound, BadRequest<ProblemDetails>>
    > GetExperiencesAsync(
        [FromServices] IQueryHandler<GetExperiences.Query, IReadOnlyList<ExperienceModel>> handler,
        [FromServices] ILanguageContext languageContext,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(
            new GetExperiences.Query(languageContext.Current),
            cancellationToken
        );
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return result.HandleFailure();
    }
}
