using Carter;
using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Abstractions.Rendering;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Kulku.Domain.Projects;
using Kulku.Web.Api.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Api.Endpoints;

public class ProjectEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("project");

        group.MapGet("", GetProjectsAsync).WithName(nameof(GetProjectsAsync));
        group.MapGet("keywords/{type}", GetKeywordsAsync).WithName(nameof(GetKeywordsAsync));
    }

    public static async Task<
        Results<Ok<IReadOnlyList<ProjectModel>>, NotFound, BadRequest<ProblemDetails>>
    > GetProjectsAsync(
        [FromServices] IQueryHandler<GetProjects.Query, IReadOnlyList<ProjectModel>> handler,
        [FromServices] ILanguageContext languageContext,
        [FromServices] IMarkdownRenderer markdownRenderer,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(
            new GetProjects.Query(languageContext.Current),
            cancellationToken
        );
        if (result.IsSuccess && result.Value is not null)
        {
            IReadOnlyList<ProjectModel> rendered =
            [
                .. result.Value.Select(p =>
                    p with
                    {
                        Description = markdownRenderer.ToHtml(p.Description),
                    }
                ),
            ];
            return TypedResults.Ok(rendered);
        }
        return result.HandleFailure();
    }

    public static async Task<
        Results<Ok<IReadOnlyList<KeywordModel>>, NotFound, BadRequest<ProblemDetails>>
    > GetKeywordsAsync(
        [FromRoute] string type,
        [FromServices] IQueryHandler<GetKeywords.Query, IReadOnlyList<KeywordModel>> handler,
        [FromServices] ILanguageContext languageContext,
        CancellationToken cancellationToken
    )
    {
        if (!Enum.TryParse<KeywordType>(type, ignoreCase: true, out var parsedType))
            return TypedResults.NotFound();

        var result = await handler.Handle(
            new GetKeywords.Query((KeywordType)parsedType, languageContext.Current),
            cancellationToken
        );
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return result.HandleFailure();
    }
}
