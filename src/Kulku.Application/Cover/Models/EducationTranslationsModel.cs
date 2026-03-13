namespace Kulku.Application.Cover.Models;

public sealed record EducationTranslationsModel(
    Guid EducationId,
    Guid InstitutionId,
    DateOnly StartDate,
    DateOnly? EndDate,
    IReadOnlyList<EducationTranslationItem> Translations,
    IReadOnlyList<InstitutionTranslationItem> InstitutionTranslations
);
