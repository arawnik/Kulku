using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Education;

/// <summary>
/// Shared validation logic for education create and update commands.
/// </summary>
internal static class EducationCommandValidator
{
    /// <summary>
    /// Validates the common fields shared by create and update education commands.
    /// Returns an empty array when the input is valid.
    /// </summary>
    public static Error[] Validate(
        DateOnly startDate,
        DateOnly? endDate,
        IReadOnlyList<EducationTranslationDto> translations
    )
    {
        List<Error> errors = [];

        if (translations.Count == 0)
            errors.Add(Error.BusinessRule("At least one translation is required."));

        if (endDate.HasValue && endDate < startDate)
            errors.Add(Error.BusinessRule("End date cannot be before start date."));

        foreach (var t in translations)
        {
            if (string.IsNullOrWhiteSpace(t.Title))
                errors.Add(
                    Error.BusinessRule($"Title is required for the {t.Language} translation.")
                );
        }

        return [.. errors];
    }
}
