using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Api.Middleware;

public static class EndpointHelper
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
            CreateErrorDetails("Not found", StatusCodes.Status404NotFound, error)
        );
    }

    public static BadRequest<ProblemDetails> HandleFailure(
        this SoulNETLib.Clean.Domain.IResult result
    ) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => TypedResults.BadRequest(
                CreateErrorDetails(
                    "Validation Error",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    [.. validationResult.Errors.Distinct()]
                )
            ),
            _ => TypedResults.BadRequest(
                CreateErrorDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error)
            ),
        };

    public static ProblemDetails CreateErrorDetails(
        string title,
        int status,
        Error? error,
        IEnumerable<Error>? validationErrors = null
    )
    {
        var problemDetails = new ProblemDetails { Title = title, Status = status };

        if (validationErrors != null)
        {
            var errorsDict = validationErrors
                .GroupBy(e => e.Code)
                .ToDictionary(g => g.Key, g => g.Select(e => e.Message).ToList());

            problemDetails.Type = error?.Code ?? ErrorCodes.Validation;
            problemDetails.Detail = error?.Message ?? IValidationResult.ValidationError.Message;
            problemDetails.Extensions["errors"] = errorsDict;
        }
        else
        {
            problemDetails.Type = error?.Code ?? ErrorCodes.General;
            problemDetails.Detail =
                error?.Message ?? "Error occurred while processing your request.";
        }

        return problemDetails;
    }
}
