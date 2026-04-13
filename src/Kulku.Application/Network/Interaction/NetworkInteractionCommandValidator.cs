using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Interaction;

/// <summary>
/// Shared validation logic for network interaction commands.
/// </summary>
internal static class NetworkInteractionCommandValidator
{
    public static Error[] Validate(string summary, bool isWarmIntro, string? referredByName)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(summary))
            errors.Add(Error.Validation(nameof(summary), "Summary is required."));

        if (isWarmIntro && string.IsNullOrWhiteSpace(referredByName))
            errors.Add(
                Error.Validation(
                    nameof(referredByName),
                    "Referred-by name is required for warm introductions."
                )
            );

        return [.. errors];
    }
}
