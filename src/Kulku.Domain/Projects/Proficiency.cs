using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a skill or competency level.
/// </summary>
/// <remarks>
/// This entity supports multilingual translations via <see cref="ProficiencyTranslation"/> records.
/// </remarks>
public class Proficiency : ITranslatableEntity<ProficiencyTranslation>
{
    /// <summary>
    /// Unique identifier for the proficiency level.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The relative scale value of this proficiency level (e.g., 100 = maximum).
    /// </summary>
    public int Scale { get; set; } = 100;

    /// <summary>
    /// Defines the display order of the proficiency level.
    /// Lower values appear first.
    /// </summary>
    public int Order { get; set; } = 1;

    /// <summary>
    /// A collection of localized translations for this proficiency record.
    /// </summary>
    public ICollection<ProficiencyTranslation> Translations { get; init; } = [];

    /// <summary>
    /// Collection of keywords that reference this proficiency level.
    /// </summary>
    public ICollection<Keyword> Keywords { get; init; } = [];
}
