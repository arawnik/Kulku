using Kulku.Application.Cover.Institution.Models;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Institution;

/// <summary>
/// Shared validation logic for institution create and update commands.
/// </summary>
internal static class InstitutionCommandValidator
{
    public static Error[] Validate(IReadOnlyList<InstitutionTranslationDto> translations)
    {
        List<Error> errors = [];

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
