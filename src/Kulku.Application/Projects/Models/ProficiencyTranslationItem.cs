using Kulku.Domain;

namespace Kulku.Application.Projects.Models;

/// <summary>
/// A single translated name and description entry for a proficiency level.
/// </summary>
public sealed record ProficiencyTranslationItem(
    LanguageCode Language,
    string Name,
    string? Description
);
