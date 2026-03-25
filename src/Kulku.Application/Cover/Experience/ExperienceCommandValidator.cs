using Kulku.Application.Cover.Experience.Models;
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
                Error.Validation(nameof(translations), "At least one translation is required.")
            );

        if (endDate.HasValue && endDate < startDate)
            errors.Add(Error.Validation(nameof(endDate), "End date cannot be before start date."));

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
