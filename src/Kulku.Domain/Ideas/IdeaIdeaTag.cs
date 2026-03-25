namespace Kulku.Domain.Ideas;

/// <summary>
/// Join entity for the many-to-many relationship between
/// <see cref="Idea"/> and <see cref="IdeaTag"/>.
/// </summary>
public class IdeaIdeaTag
{
    /// <summary>
    /// Foreign key to the idea.
    /// </summary>
    public Guid IdeaId { get; set; }

    /// <summary>
    /// Foreign key to the tag.
    /// </summary>
    public Guid IdeaTagId { get; set; }

    /// <summary>
    /// Navigation property to the idea.
    /// </summary>
    public Idea Idea { get; set; } = null!;

    /// <summary>
    /// Navigation property to the tag.
    /// </summary>
    public IdeaTag IdeaTag { get; set; } = null!;
}
