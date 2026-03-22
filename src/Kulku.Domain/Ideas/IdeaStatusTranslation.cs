using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Ideas;

/// <summary>
/// Localized translation for an <see cref="IdeaStatus"/> entity.
/// </summary>
public class IdeaStatusTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for the translation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key to the associated idea status.
    /// </summary>
    public Guid IdeaStatusId { get; set; }

    /// <summary>
    /// Localized display name of the status.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional localized description of the status.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the parent <see cref="IdeaStatus"/>.
    /// </summary>
    public IdeaStatus IdeaStatus { get; set; } = null!;
}
