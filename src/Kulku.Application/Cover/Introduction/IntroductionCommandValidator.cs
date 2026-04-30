using System.Globalization;
using Kulku.Application.Cover.Introduction.Models;
using Kulku.Application.Resources;
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
                Error.Validation(nameof(translations), Strings.Validation_TranslationsRequired)
            );

        if (string.IsNullOrWhiteSpace(avatarUrl))
            errors.Add(
                Error.Validation(nameof(avatarUrl), Strings.Validation_AvatarFilenameRequired)
            );

        if (string.IsNullOrWhiteSpace(smallAvatarUrl))
            errors.Add(
                Error.Validation(
                    nameof(smallAvatarUrl),
                    Strings.Validation_SmallAvatarFilenameRequired
                )
            );

        for (var i = 0; i < translations.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(translations[i].Title))
                errors.Add(
                    Error.Validation(
                        $"{nameof(translations)}[{i}].Title",
                        string.Format(
                            CultureInfo.InvariantCulture,
                            Strings.Validation_TitleRequiredForLanguage,
                            translations[i].Language
                        )
                    )
                );
        }

        return [.. errors];
    }
}
