using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Ideas;

/// <summary>
/// A top-level categorization bucket for ideas (e.g. Career Growth, Game Dev).
/// Seeded via database initializer; not user-manageable.
/// </summary>
public class IdeaDomain : ITranslatableEntity<IdeaDomainTranslation>
{
    /// <summary>
    /// Unique identifier for the domain.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Bootstrap icon name for UI display.
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Display order (lower = higher priority).
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Localized translations for this domain.
    /// </summary>
    public ICollection<IdeaDomainTranslation> Translations { get; init; } = [];

    /// <summary>
    /// Ideas categorized under this domain.
    /// </summary>
    public ICollection<Idea> Ideas { get; init; } = [];
}
