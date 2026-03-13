using Kulku.Domain;

namespace Kulku.Application.Cover.Models;

/// <summary>
/// Read model for an institution with all its translations.
/// </summary>
public sealed record InstitutionTranslationsModel(
    Guid InstitutionId,
    IReadOnlyList<InstitutionTranslationItem> Translations
);

public sealed record InstitutionTranslationItem(
    LanguageCode Language,
    string Name,
    string? Department,
    string Description
);
