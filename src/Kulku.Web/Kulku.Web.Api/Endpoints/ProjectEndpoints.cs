using Carter;
using Kulku.Application.Projects;
using Kulku.Contract.Enums;
using Kulku.Contract.Projects;
using Kulku.Web.Api.Middleware;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Common.Extension;

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
        Results<Ok<ICollection<ProjectResponse>>, NotFound, BadRequest<ProblemDetails>>
    > GetProjectsAsync(
        [FromServices] IQueryHandler<GetProjects.Query, ICollection<ProjectResponse>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(new GetProjects.Query(), cancellationToken);
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return result.HandleFailure();
    }

    public static async Task<
        Results<Ok<ICollection<KeywordResponse>>, NotFound, BadRequest<ProblemDetails>>
    > GetKeywordsAsync(
        [FromRoute] string type,
        [FromServices] IQueryHandler<GetKeywords.Query, ICollection<KeywordResponse>> handler,
        CancellationToken cancellationToken
    )
    {
        if (!type.TryParseEnumMember(out KeywordType? parsedType))
            return TypedResults.NotFound();

        var result = await handler.Handle(
            new GetKeywords.Query((KeywordType)parsedType),
            cancellationToken
        );
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return result.HandleFailure();
    }
}
