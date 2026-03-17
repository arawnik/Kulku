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
                Error.Validation(nameof(translations), "At least one translation is required.")
            );

        if (url is null || string.IsNullOrWhiteSpace(url.ToString()))
            errors.Add(Error.Validation(nameof(url), "Project URL is required."));

        if (string.IsNullOrWhiteSpace(imageUrl))
            errors.Add(Error.Validation(nameof(imageUrl), "Image filename is required."));

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
