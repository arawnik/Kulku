using System.Globalization;
using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Shared validation logic for proficiency create and update commands.
/// </summary>
internal static class ProficiencyCommandValidator
{
    public static Error[] Validate(int scale, IReadOnlyList<ProficiencyTranslationDto> translations)
    {
        List<Error> errors = [];

        if (scale < 0 || scale > 100)
            errors.Add(Error.Validation(nameof(scale), Strings.Validation_ScaleRange));

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
