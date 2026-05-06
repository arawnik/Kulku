using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Contact;

/// <summary>
/// Shared validation rules for network contact create and update commands.
/// </summary>
internal static class NetworkContactUpsertRules
{
    public static Error[] Validate(string? personName, string? email, string? linkedInUrl)
    {
        List<Error> errors = [];

        if (
            string.IsNullOrWhiteSpace(personName)
            && string.IsNullOrWhiteSpace(email)
            && string.IsNullOrWhiteSpace(linkedInUrl)
        )
        {
            errors.Add(
                Error.Validation(nameof(personName), Strings.Validation_ContactIdentifyingField)
            );
        }

        return [.. errors];
    }
}
