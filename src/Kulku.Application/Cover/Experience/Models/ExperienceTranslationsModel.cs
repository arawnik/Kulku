using Kulku.Domain;

namespace Kulku.Application.Cover.Experience.Models;

public sealed record ExperienceTranslationItem(
    LanguageCode Language,
    string Title,
    string Description
);

public sealed record CompanyTranslationItem(LanguageCode Language, string Name, string Description);

public sealed record ExperienceTranslationsModel(
    Guid ExperienceId,
    Guid CompanyId,
    DateOnly StartDate,
    DateOnly? EndDate,
    IReadOnlyList<ExperienceTranslationItem> Translations,
    IReadOnlyList<CompanyTranslationItem> CompanyTranslations,
    IReadOnlyList<string> KeywordNames
);
