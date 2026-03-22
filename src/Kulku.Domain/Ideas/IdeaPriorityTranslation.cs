using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Ideas;

/// <summary>
/// Localized translation for an <see cref="IdeaPriority"/> entity.
/// </summary>
public class IdeaPriorityTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for the translation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key to the associated idea priority.
    /// </summary>
    public Guid IdeaPriorityId { get; set; }

    /// <summary>
    /// Localized display name of the priority.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional localized description of the priority.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the parent <see cref="IdeaPriority"/>.
    /// </summary>
    public IdeaPriority IdeaPriority { get; set; } = null!;
}
