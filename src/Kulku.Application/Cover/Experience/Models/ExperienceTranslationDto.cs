using Kulku.Domain;
using Kulku.Domain.Abstractions;

namespace Kulku.Application.Cover.Experience.Models;

/// <summary>
/// Represents translated content for an experience entry in a specific language.
/// Shared across create and update commands.
/// </summary>
public sealed record ExperienceTranslationDto(
    LanguageCode Language,
    string Title,
    string Description
) : ITranslationDto;
