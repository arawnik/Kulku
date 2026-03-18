using Carter;
using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Abstractions.Rendering;
using Kulku.Application.Cover.Introduction;
using Kulku.Application.Cover.Models;
using Kulku.Web.Api.Http;
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
        Results<Ok<IntroductionModel>, NotFound, BadRequest<ProblemDetails>>
    > GetIntroductionAsync(
        [FromServices] IQueryHandler<GetIntroduction.Query, IntroductionModel?> handler,
        [FromServices] ILanguageContext languageContext,
        [FromServices] IMarkdownRenderer markdownRenderer,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(
            new GetIntroduction.Query(languageContext.Current),
            cancellationToken
        );
        if (result.IsSuccess && result.Value is not null)
        {
            var rendered = result.Value with
            {
                Content = markdownRenderer.ToHtml(result.Value.Content),
            };
            return TypedResults.Ok(rendered);
        }
        return result.HandleFailure();
    }
}
