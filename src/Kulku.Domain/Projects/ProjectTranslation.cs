using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a localized version of a project entity, containing language-specific data.
/// </summary>
/// <remarks>
/// Used to provide language-specific values for project.
/// </remarks>
public class ProjectTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for the project translation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key reference to the parent project.
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// The localized name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A short, localized summary or tagline for the project.
    /// </summary>
    public string Info { get; set; } = string.Empty;

    /// <summary>
    /// A detailed, localized description of the project.
    /// Can be left empty.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property for the related <see cref="Project"/> entity.
    /// </summary>
    public Project Project { get; set; } = null!;
}
