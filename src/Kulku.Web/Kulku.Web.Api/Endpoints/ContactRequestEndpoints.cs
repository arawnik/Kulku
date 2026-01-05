using Carter;
using Kulku.Application.Contacts;
using Kulku.Application.Contacts.Models;
using Kulku.Web.Api.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Api.Endpoints;

public class ContactRequestEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("contact");

        group.MapPost("", SubmitContactRequestAsync).WithName(nameof(SubmitContactRequestAsync));
    }

    public static async Task<
        Results<NoContent, BadRequest<ProblemDetails>>
    > SubmitContactRequestAsync(
        ContactRequestDto dto,
        [FromServices] ICommandHandler<SubmitContactRequest.Command> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(new SubmitContactRequest.Command(dto), cancellationToken);
        if (result.IsSuccess)
        {
            return TypedResults.NoContent();
        }
        return result.HandleFailure();
    }
}
