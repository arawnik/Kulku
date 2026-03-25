namespace Kulku.Application.Cover.Introduction.Models;

/// <summary>
/// Admin read model for an introduction with all its translations.
/// Used by admin views that display and edit content across all languages.
/// </summary>
public sealed record IntroductionTranslationsModel(
    Guid IntroductionId,
    string AvatarUrl,
    string SmallAvatarUrl,
    DateTime PubDate,
    IReadOnlyList<IntroductionTranslationItem> Translations
);
