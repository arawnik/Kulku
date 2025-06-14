using Carter;
using Kulku.Application.Cover;
using Kulku.Contract.Cover;
using Kulku.Web.Api.Middleware;
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
        Results<Ok<ICollection<EducationResponse>>, NotFound, BadRequest<ProblemDetails>>
    > GetEducationsAsync(
        [FromServices] IQueryHandler<GetEducations.Query, ICollection<EducationResponse>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(new GetEducations.Query(), cancellationToken);
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }
        return result.HandleFailure();
    }
}
