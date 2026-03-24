using Kulku.Domain;

namespace Kulku.Application.Cover.Institution.Models;

/// <summary>
/// A single translated entry for an institution.
/// </summary>
public sealed record InstitutionTranslationDto(
    LanguageCode Language,
    string Name,
    string? Department,
    string Description
);
