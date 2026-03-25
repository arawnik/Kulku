namespace Kulku.Application.Cover.Models;

/// <summary>
/// Read model for an institution with all its translations.
/// </summary>
public sealed record InstitutionTranslationsModel(
    Guid InstitutionId,
    int EducationCount,
    IReadOnlyList<InstitutionTranslationItem> Translations
);
