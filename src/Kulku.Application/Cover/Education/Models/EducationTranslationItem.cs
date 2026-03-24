using Kulku.Domain;

namespace Kulku.Application.Cover.Education.Models;

/// <summary>
/// A single translated entry for an education.
/// </summary>
public sealed record EducationTranslationItem(
    LanguageCode Language,
    string Title,
    string Description
);
