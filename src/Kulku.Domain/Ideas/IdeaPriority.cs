using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Ideas;

/// <summary>
/// Represents a priority level for ideas (e.g. Low, Medium, High).
/// Seeded via database initializer; not user-manageable.
/// </summary>
public class IdeaPriority : ITranslatableEntity<IdeaPriorityTranslation>
{
    /// <summary>
    /// Unique identifier for the priority level.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display order (lower = lower priority).
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// CSS class for badge styling (e.g. "bg-danger", "bg-warning text-dark").
    /// </summary>
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Localized translations for this priority level.
    /// </summary>
    public ICollection<IdeaPriorityTranslation> Translations { get; init; } = [];

    /// <summary>
    /// Ideas currently assigned this priority level.
    /// </summary>
    public ICollection<Idea> Ideas { get; init; } = [];
}
