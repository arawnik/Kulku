using Kulku.Application.Cover.Models;

namespace Kulku.Application.Cover.Experience.Models;

/// <summary>
/// Read model for an experience entry with all its translations and related company translations.
/// Used by admin views that display and edit content across all languages.
/// </summary>
public sealed record ExperienceTranslationsModel(
    Guid ExperienceId,
    Guid CompanyId,
    DateOnly StartDate,
    DateOnly? EndDate,
    IReadOnlyList<ExperienceTranslationItem> Translations,
    IReadOnlyList<CompanyTranslationItem> CompanyTranslations,
    IReadOnlyList<string> KeywordNames
);
