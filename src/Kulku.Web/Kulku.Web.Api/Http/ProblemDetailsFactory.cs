using Microsoft.AspNetCore.Mvc;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Api.Http;

public static class ProblemDetailsFactory
{
    public static ProblemDetails Create(
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
