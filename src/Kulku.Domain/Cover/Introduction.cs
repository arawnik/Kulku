using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents the introduction section of a CV.
/// </summary>
/// <remarks>
/// This entity supports multilingual translations via <see cref="IntroductionTranslation"/> records.
/// </remarks>
public class Introduction : ITranslatableEntity<IntroductionTranslation>
{
    /// <summary>
    /// Unique identifier for the introduction.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// URL to the full-size avatar image.
    /// </summary>
    public required Uri AvatarUrl { get; set; }

    /// <summary>
    /// URL to the small avatar image.
    /// </summary>
    public required Uri SmallAvatarUrl { get; set; }

    /// <summary>
    /// Publication timestamp for the introduction content.
    /// </summary>
    public DateTime PubDate { get; set; }

    /// <summary>
    /// A collection of localized translations for this introduction record.
    /// </summary>
    public ICollection<IntroductionTranslation> Translations { get; init; } = [];
}
