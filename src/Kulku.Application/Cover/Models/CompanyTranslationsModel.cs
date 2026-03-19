using Kulku.Domain;

namespace Kulku.Application.Cover.Models;

/// <summary>
/// Read model for a company with all its translations.
/// </summary>
public sealed record CompanyTranslationsModel(
    Guid CompanyId,
    int ExperienceCount,
    IReadOnlyList<CompanyTranslationItem> Translations
);

public sealed record CompanyTranslationItem(LanguageCode Language, string Name, string Description);
