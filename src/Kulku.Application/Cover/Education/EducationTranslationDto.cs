using Kulku.Domain;

namespace Kulku.Application.Cover.Education;

/// <summary>
/// Represents translated content for an education entry in a specific language.
/// Shared across create and update commands.
/// </summary>
public sealed record EducationTranslationDto(
    LanguageCode Language,
    string Title,
    string Description
);
