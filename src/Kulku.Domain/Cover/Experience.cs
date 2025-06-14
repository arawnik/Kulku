using Kulku.Domain.Abstractions;
using Kulku.Domain.Projects;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents a single work experience entry.
/// </summary>
/// <remarks>
/// This entity supports multilingual translations via <see cref="ExperienceTranslation"/> records.
/// </remarks>
public class Experience : ITranslatableEntity<ExperienceTranslation>
{
    /// <summary>
    /// Unique identifier for this experience.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the associated <see cref="Company"/>.
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// The start date of the experience.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// The (optional) end date of the experience.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// A collection of localized translations for this experience record.
    /// </summary>
    public ICollection<ExperienceTranslation> Translations { get; init; } = [];

    /// <summary>
    /// Keywords associated with this experience (e.g., skills or technologies used).
    /// </summary>
    public ICollection<Keyword> Keywords { get; init; } = [];

    /// <summary>
    /// Navigation property to the associated company.
    /// </summary>
    public Company Company { get; set; } = null!;
}
