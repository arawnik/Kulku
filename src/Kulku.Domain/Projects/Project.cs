using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a portfolio project.
/// </summary>
/// <remarks>
/// This entity supports multilingual translations via <see cref="ProjectTranslation"/> records.
/// </remarks>
public class Project : ITranslatableEntity<ProjectTranslation>
{
    /// <summary>
    /// Unique identifier for the project.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The primary URL associated with the project, typically linking to a live site or repository.
    /// </summary>
    public required Uri Url { get; set; }

    /// <summary>
    /// Defines the display order of the project in lists or views.
    /// Lower values indicate higher priority.
    /// </summary>
    public int Order { get; set; } = 1;

    /// <summary>
    /// Image filename or path representing the project (e.g. "Mietekallio.webp").
    /// Presentation layers resolve this to a full URL using their own conventions.
    /// </summary>
    public required string ImageUrl { get; set; }

    /// <summary>
    /// A collection of localized translations for this project record.
    /// </summary>
    public ICollection<ProjectTranslation> Translations { get; init; } = [];

    /// <summary>
    /// Keywords associated with this project.
    /// </summary>
    public ICollection<ProjectKeyword> ProjectKeywords { get; init; } = [];
}
