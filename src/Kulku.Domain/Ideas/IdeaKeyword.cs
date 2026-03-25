using Kulku.Domain.Projects;

namespace Kulku.Domain.Ideas;

/// <summary>
/// Represents the association between an <see cref="Idea"/> and a <see cref="Keyword"/>.
/// Enables linking ideas to existing CV keywords (skills, technologies, languages).
/// </summary>
public class IdeaKeyword
{
    /// <summary>
    /// Foreign key referencing the associated idea.
    /// </summary>
    public Guid IdeaId { get; set; }

    /// <summary>
    /// Navigation property to the associated <see cref="Idea"/>.
    /// </summary>
    public Idea Idea { get; set; } = null!;

    /// <summary>
    /// Foreign key referencing the associated keyword.
    /// </summary>
    public Guid KeywordId { get; set; }

    /// <summary>
    /// Navigation property to the associated <see cref="Keyword"/>.
    /// </summary>
    public Keyword Keyword { get; set; } = null!;
}
