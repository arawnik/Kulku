namespace Kulku.Domain.Ideas;

/// <summary>
/// Represents a developer career idea that can be captured, categorized,
/// prioritized, and evolved over time through notes.
/// </summary>
public class Idea
{
    /// <summary>
    /// Unique identifier for the idea.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Short, descriptive title for the idea.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional one-liner hook or intent summary for the idea.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Detailed description of the idea in markdown format.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Foreign key referencing the current lifecycle status.
    /// </summary>
    public Guid StatusId { get; set; }

    /// <summary>
    /// Foreign key referencing the priority level for ordering and filtering.
    /// </summary>
    public Guid PriorityId { get; set; }

    /// <summary>
    /// Foreign key referencing the categorization domain.
    /// </summary>
    public Guid DomainId { get; set; }

    /// <summary>
    /// UTC timestamp when the idea was first created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp of the last modification to the idea.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property to the categorization domain.
    /// </summary>
    public IdeaDomain Domain { get; set; } = null!;

    /// <summary>
    /// Navigation property to the lifecycle status.
    /// </summary>
    public IdeaStatus Status { get; set; } = null!;

    /// <summary>
    /// Navigation property to the priority level.
    /// </summary>
    public IdeaPriority Priority { get; set; } = null!;

    /// <summary>
    /// Timestamped notes capturing evolving thinking on this idea.
    /// </summary>
    public ICollection<IdeaNote> Notes { get; init; } = [];

    /// <summary>
    /// Many-to-many join entries linking this idea to tags.
    /// </summary>
    public ICollection<IdeaIdeaTag> IdeaIdeaTags { get; init; } = [];

    /// <summary>
    /// Many-to-many join entries linking this idea to CV keywords.
    /// </summary>
    public ICollection<IdeaKeyword> IdeaKeywords { get; init; } = [];
}
