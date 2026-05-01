using System.Globalization;
using Kulku.Application.Resources;
using Kulku.Domain.Projects;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Shared validation logic for keyword create and update commands.
/// </summary>
internal static class KeywordCommandValidator
{
    public static Error[] Validate(
        KeywordType type,
        Guid proficiencyId,
        IReadOnlyList<KeywordTranslationDto> translations
    )
    {
        List<Error> errors = [];

        if (proficiencyId == Guid.Empty)
            errors.Add(
                Error.Validation(nameof(proficiencyId), Strings.Validation_ProficiencyRequired)
            );

        if (translations.Count == 0)
            errors.Add(
                Error.Validation(nameof(translations), Strings.Validation_TranslationsRequired)
            );

        for (var i = 0; i < translations.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(translations[i].Name))
                errors.Add(
                    Error.Validation(
                        $"{nameof(translations)}[{i}].Name",
                        string.Format(
                            CultureInfo.InvariantCulture,
                            Strings.Validation_NameRequiredForLanguage,
                            translations[i].Language
                        )
                    )
                );
        }

        return [.. errors];
    }
}
