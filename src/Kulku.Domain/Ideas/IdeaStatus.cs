using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Ideas;

/// <summary>
/// Represents a lifecycle status for ideas (e.g. Spark, Exploring, Done).
/// Seeded via database initializer; not user-manageable.
/// </summary>
public class IdeaStatus : ITranslatableEntity<IdeaStatusTranslation>
{
    /// <summary>
    /// Unique identifier for the status.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display order (lower = earlier in lifecycle).
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// CSS class for badge styling (e.g. "bg-info", "bg-primary").
    /// </summary>
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Localized translations for this status.
    /// </summary>
    public ICollection<IdeaStatusTranslation> Translations { get; init; } = [];

    /// <summary>
    /// Ideas currently assigned this status.
    /// </summary>
    public ICollection<Idea> Ideas { get; init; } = [];
}
