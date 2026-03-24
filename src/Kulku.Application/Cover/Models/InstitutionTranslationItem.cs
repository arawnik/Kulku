using Kulku.Domain;

namespace Kulku.Application.Cover.Models;

/// <summary>
/// A single translated content entry for an institution.
/// </summary>
public sealed record InstitutionTranslationItem(
    LanguageCode Language,
    string Name,
    string? Department,
    string Description
);
