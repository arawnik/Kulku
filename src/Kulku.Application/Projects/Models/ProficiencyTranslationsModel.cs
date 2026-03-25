namespace Kulku.Application.Projects.Models;

/// <summary>
/// Admin read model for a proficiency level with all its translations.
/// </summary>
public sealed record ProficiencyTranslationsModel(
    Guid ProficiencyId,
    int Scale,
    int Order,
    int KeywordCount,
    IReadOnlyList<ProficiencyTranslationItem> Translations
);
