using Kulku.Domain;

namespace Kulku.Application.Cover.Introduction.Models;

/// <summary>
/// Input DTO for introduction translation data in create/update commands.
/// </summary>
public sealed record IntroductionTranslationDto(
    LanguageCode Language,
    string Title,
    string Tagline,
    string Content
);
