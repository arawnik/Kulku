using System.Globalization;
using Kulku.Application.Projects.Models;
using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Shared validation logic for project create and update commands.
/// </summary>
internal static class ProjectCommandValidator
{
    /// <summary>
    /// Validates the common fields shared by create and update project commands.
    /// Returns an empty array when the input is valid.
    /// </summary>
    public static Error[] Validate(
        Uri? url,
        string? imageUrl,
        IReadOnlyList<ProjectTranslationDto> translations
    )
    {
        List<Error> errors = [];

        if (translations.Count == 0)
            errors.Add(
                Error.Validation(nameof(translations), Strings.Validation_TranslationsRequired)
            );

        if (url is null || string.IsNullOrWhiteSpace(url.ToString()))
            errors.Add(Error.Validation(nameof(url), Strings.Validation_ProjectUrlRequired));

        if (string.IsNullOrWhiteSpace(imageUrl))
            errors.Add(
                Error.Validation(nameof(imageUrl), Strings.Validation_ImageFilenameRequired)
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
