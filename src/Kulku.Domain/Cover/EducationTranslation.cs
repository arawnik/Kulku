using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents the localized content of an <see cref="Education"/> entry.
/// </summary>
/// <remarks>
/// Used to provide language-specific values for education.
/// </remarks>
public class EducationTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for this translation entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the parent <see cref="Education"/>.
    /// </summary>
    public Guid EducationId { get; set; }

    /// <summary>
    /// The localized title for the education entry.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The localized description for the education entry.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the associated <see cref="Education"/> entity.
    /// </summary>
    public Education Education { get; set; } = null!;
}
