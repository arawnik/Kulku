using Kulku.Domain;

namespace Kulku.Application.Cover.Experience.Models;

/// <summary>
/// A single translated title and description for an experience entry.
/// </summary>
public sealed record ExperienceTranslationItem(
    LanguageCode Language,
    string Title,
    string Description
);
