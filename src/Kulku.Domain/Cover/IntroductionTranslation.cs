using Kulku.Contract.Enums;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents the localized content of an <see cref="Introduction"/> entry.
/// </summary>
/// <remarks>
/// Used to provide language-specific values for introduction.
/// </remarks>
public class IntroductionTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for the translation entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the associated <see cref="Introduction"/>.
    /// </summary>
    public Guid IntroductionId { get; set; }

    /// <summary>
    /// Localized title for the introduction.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Localized content body for the introduction.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Localized tagline.
    /// </summary>
    public string Tagline { get; set; } = string.Empty;

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the parent <see cref="Introduction"/>.
    /// </summary>
    public Introduction Introduction { get; set; } = null!;
}
