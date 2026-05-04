using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Contacts;

/// <summary>
/// Shared validation logic for contact request commands.
/// </summary>
internal static class ContactRequestCommandValidator
{
    public static Error[] Validate(
        string? name,
        string? email,
        string? subject,
        string? message,
        string? captchaToken
    )
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(Error.Validation(nameof(name), Strings.Validation_ContactNameRequired));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(Error.Validation(nameof(email), Strings.Validation_ContactEmailRequired));
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            errors.Add(
                Error.Validation(nameof(subject), Strings.Validation_ContactSubjectRequired)
            );
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            errors.Add(
                Error.Validation(nameof(message), Strings.Validation_ContactMessageRequired)
            );
        }

        if (string.IsNullOrWhiteSpace(captchaToken))
        {
            errors.Add(
                Error.Validation(nameof(captchaToken), Strings.Validation_CaptchaTokenRequired)
            );
        }

        return [.. errors];
    }
}
