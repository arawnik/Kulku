using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Interaction;

/// <summary>
/// Shared validation rules for network interaction create and update commands.
/// </summary>
internal static class NetworkInteractionUpsertRules
{
    public static Error[] Validate(string summary, bool isWarmIntro, string? referredByName)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(summary))
            errors.Add(Error.Validation(nameof(summary), Strings.Validation_SummaryRequired));

        if (isWarmIntro && string.IsNullOrWhiteSpace(referredByName))
            errors.Add(
                Error.Validation(nameof(referredByName), Strings.Validation_ReferredByNameRequired)
            );

        return [.. errors];
    }
}
