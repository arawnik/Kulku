namespace Kulku.Domain.Ideas;

/// <summary>
/// A timestamped note entry attached to an idea, used to
/// capture evolving thinking over time.
/// </summary>
public class IdeaNote
{
    /// <summary>
    /// Unique identifier for the note.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the parent idea.
    /// </summary>
    public Guid IdeaId { get; set; }

    /// <summary>
    /// Note content in markdown format.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the note was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property to the parent idea.
    /// </summary>
    public Idea Idea { get; set; } = null!;
}
