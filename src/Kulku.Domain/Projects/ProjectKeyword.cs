namespace Kulku.Domain.Projects;

/// <summary>
/// Represents the association between a <see cref="Project"/> and a <see cref="Keyword"/>.
/// Enables a many-to-many relationship for tagging projects with relevant keywords.
/// </summary>
public class ProjectKeyword
{
    /// <summary>
    /// Foreign key referencing the associated project.
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Navigation property to the associated <see cref="Project"/>.
    /// </summary>
    public Project Project { get; set; } = null!;

    /// <summary>
    /// Foreign key referencing the associated keyword.
    /// </summary>
    public Guid KeywordId { get; set; }

    /// <summary>
    /// Navigation property to the associated <see cref="Keyword"/>.
    /// </summary>
    public Keyword Keyword { get; set; } = null!;
}
