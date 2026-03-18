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
                Error.Validation(nameof(proficiencyId), "A proficiency level must be selected.")
            );

        if (translations.Count == 0)
            errors.Add(
                Error.Validation(nameof(translations), "At least one translation is required.")
            );

        for (var i = 0; i < translations.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(translations[i].Name))
                errors.Add(
                    Error.Validation(
                        $"{nameof(translations)}[{i}].Name",
                        $"Name is required for the {translations[i].Language} translation."
                    )
                );
        }

        return [.. errors];
    }
}
