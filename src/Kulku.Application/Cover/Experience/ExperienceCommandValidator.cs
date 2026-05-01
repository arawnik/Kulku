using System.Globalization;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Experience;

/// <summary>
/// Shared validation logic for experience create and update commands.
/// </summary>
internal static class ExperienceCommandValidator
{
    /// <summary>
    /// Validates the common fields shared by create and update experience commands.
    /// Returns an empty array when the input is valid.
    /// </summary>
    public static Error[] Validate(
        DateOnly startDate,
        DateOnly? endDate,
        IReadOnlyList<ExperienceTranslationDto> translations
    )
    {
        List<Error> errors = [];

        if (translations.Count == 0)
            errors.Add(
                Error.Validation(nameof(translations), Strings.Validation_TranslationsRequired)
            );

        if (endDate.HasValue && endDate < startDate)
            errors.Add(Error.Validation(nameof(endDate), Strings.Validation_EndDateBeforeStart));

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
