using Kulku.Domain.Projects;

namespace Kulku.Application.Projects.Models;

/// <summary>
/// Admin read model for a keyword with all its translations and proficiency info.
/// </summary>
public sealed record KeywordTranslationsModel(
    Guid KeywordId,
    KeywordType Type,
    int Order,
    bool Display,
    Guid ProficiencyId,
    string ProficiencyName,
    int ProficiencyScale,
    IReadOnlyList<KeywordTranslationItem> Translations
);
