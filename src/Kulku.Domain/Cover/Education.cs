using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents a record of educational experience.
/// </summary>
/// <remarks>
/// This entity supports multilingual translations via <see cref="EducationTranslation"/> records.
/// </remarks>
public class Education : ITranslatableEntity<EducationTranslation>
{
    /// <summary>
    /// Unique identifier for the education entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the institution where the education took place.
    /// </summary>
    public Guid InstitutionId { get; set; }

    /// <summary>
    /// The start date of the education period.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// The (optional) end date of the education period.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// A collection of localized translations for this education record.
    /// </summary>
    public ICollection<EducationTranslation> Translations { get; init; } = [];

    /// <summary>
    /// Navigation property to the related institution.
    /// </summary>
    public Institution Institution { get; set; } = null!;
}
