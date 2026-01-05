using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Api.Http;

public static class ResultHttpExtensions
{
    public static Results<
        Ok<TSuccess>,
        BadRequest<ProblemDetails>,
        NotFound<ProblemDetails>
    > HandleFailureWith404<TSuccess>(this Result<TSuccess> result)
    {
        return string.Equals(result.Error?.Code, ErrorCodes.NotFound, StringComparison.Ordinal)
            ? Handle404(result.Error)
            : HandleFailure(result);
    }

    public static Results<
        NoContent,
        BadRequest<ProblemDetails>,
        NotFound<ProblemDetails>
    > HandleFailureWith404(this SoulNETLib.Clean.Domain.IResult result)
    {
        return string.Equals(result.Error?.Code, ErrorCodes.NotFound, StringComparison.Ordinal)
            ? Handle404(result.Error)
            : result.HandleFailure();
    }

    private static NotFound<ProblemDetails> Handle404(Error? error)
    {
        return TypedResults.NotFound(
            ProblemDetailsFactory.Create("Not found", StatusCodes.Status404NotFound, error)
        );
    }

    public static BadRequest<ProblemDetails> HandleFailure(
        this SoulNETLib.Clean.Domain.IResult result
    ) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => TypedResults.BadRequest(
                ProblemDetailsFactory.Create(
                    "Validation Error",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    [.. validationResult.Errors.Distinct()]
                )
            ),
            _ => TypedResults.BadRequest(
                ProblemDetailsFactory.Create(
                    "Bad Request",
                    StatusCodes.Status400BadRequest,
                    result.Error
                )
            ),
        };
}
