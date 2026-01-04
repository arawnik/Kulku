using Carter;
using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover;
using Kulku.Application.Cover.Models;
using Kulku.Web.Api.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Api.Endpoints;

public class EducationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("education");

        group.MapGet("", GetEducationsAsync).WithName(nameof(GetEducationsAsync));
    }

    public static async Task<
        Results<Ok<IReadOnlyList<EducationModel>>, NotFound, BadRequest<ProblemDetails>>
    > GetEducationsAsync(
        [FromServices] IQueryHandler<GetEducations.Query, IReadOnlyList<EducationModel>> handler,
        [FromServices] ILanguageContext languageContext,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(
            new GetEducations.Query(languageContext.Current),
            cancellationToken
        );
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return result.HandleFailure();
    }
}
