using Kulku.Application.Cover.Introduction.Models;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Introduction;

/// <summary>
/// Shared validation logic for introduction create and update commands.
/// </summary>
internal static class IntroductionCommandValidator
{
    public static Error[] Validate(
        string? avatarUrl,
        string? smallAvatarUrl,
        IReadOnlyList<IntroductionTranslationDto> translations
    )
    {
        List<Error> errors = [];

        if (translations.Count == 0)
            errors.Add(
                Error.Validation(nameof(translations), "At least one translation is required.")
            );

        if (string.IsNullOrWhiteSpace(avatarUrl))
            errors.Add(Error.Validation(nameof(avatarUrl), "Avatar filename is required."));

        if (string.IsNullOrWhiteSpace(smallAvatarUrl))
            errors.Add(
                Error.Validation(nameof(smallAvatarUrl), "Small avatar filename is required.")
            );

        for (var i = 0; i < translations.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(translations[i].Title))
                errors.Add(
                    Error.Validation(
                        $"{nameof(translations)}[{i}].Title",
                        $"Title is required for the {translations[i].Language} translation."
                    )
                );
        }

        return [.. errors];
    }
}
