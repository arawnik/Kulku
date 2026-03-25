using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Ideas;

/// <summary>
/// Localized translation for an <see cref="IdeaDomain"/> entity.
/// </summary>
public class IdeaDomainTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for the translation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key to the associated idea domain.
    /// </summary>
    public Guid IdeaDomainId { get; set; }

    /// <summary>
    /// Localized display name of the domain.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the parent <see cref="IdeaDomain"/>.
    /// </summary>
    public IdeaDomain IdeaDomain { get; set; } = null!;
}
