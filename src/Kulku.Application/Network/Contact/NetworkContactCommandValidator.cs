using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Contact;

/// <summary>
/// Shared validation logic for network contact commands.
/// </summary>
internal static class NetworkContactCommandValidator
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
                Error.Validation(
                    nameof(personName),
                    "At least one identifying field (name, email, or LinkedIn) is required."
                )
            );
        }

        return [.. errors];
    }
}
