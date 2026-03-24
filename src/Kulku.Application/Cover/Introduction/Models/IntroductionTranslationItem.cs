using Kulku.Domain;

namespace Kulku.Application.Cover.Introduction.Models;

/// <summary>
/// A single translated content entry for an introduction.
/// </summary>
public sealed record IntroductionTranslationItem(
    LanguageCode Language,
    string Title,
    string Tagline,
    string Content
);
