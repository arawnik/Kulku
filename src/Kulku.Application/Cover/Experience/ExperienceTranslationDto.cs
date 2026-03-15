using Kulku.Domain;

namespace Kulku.Application.Cover.Experience;

/// <summary>
/// Represents translated content for an experience entry in a specific language.
/// Shared across create and update commands.
/// </summary>
public sealed record ExperienceTranslationDto(
    LanguageCode Language,
    string Title,
    string Description
);
