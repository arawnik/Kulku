using Kulku.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Input DTO for proficiency translation data in create/update commands.
/// </summary>
public sealed record ProficiencyTranslationDto(
    LanguageCode Language,
    string Name,
    string? Description
);
