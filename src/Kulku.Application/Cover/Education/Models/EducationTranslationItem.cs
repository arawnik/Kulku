using Kulku.Domain;

namespace Kulku.Application.Cover.Education.Models;

/// <summary>
/// A single translated title and description for an education entry.
/// </summary>
public sealed record EducationTranslationItem(
    LanguageCode Language,
    string Title,
    string Description
);
