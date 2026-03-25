using Kulku.Application.Cover.Models;

namespace Kulku.Application.Cover.Education.Models;

/// <summary>
/// Read model for an education entry with all its translations and related institution translations.
/// Used by admin views that display and edit content across all languages.
/// </summary>
public sealed record EducationTranslationsModel(
    Guid EducationId,
    Guid InstitutionId,
    DateOnly StartDate,
    DateOnly? EndDate,
    IReadOnlyList<EducationTranslationItem> Translations,
    IReadOnlyList<InstitutionTranslationItem> InstitutionTranslations
);
