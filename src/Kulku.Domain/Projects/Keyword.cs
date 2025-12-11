using Kulku.Contract.Enums;
using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a categorized descriptor keyword.
/// </summary>
/// <remarks>
/// This entity supports multilingual translations via <see cref="KeywordTranslation"/> records.
/// </remarks>
public class Keyword : ITranslatableEntity<KeywordTranslation>
{
    /// <summary>
    /// Unique identifier for the keyword.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The type of the keyword.
    /// </summary>
    public KeywordType Type { get; set; } = KeywordType.Skill;

    /// <summary>
    /// Defines the display order of the keyword in UI listings.
    /// Lower values indicate higher priority.
    /// </summary>
    public int Order { get; set; } = 1;

    /// <summary>
    /// Indicates whether this keyword should be visible in public-facing views.
    /// </summary>
    public bool Display { get; set; } = true;

    /// <summary>
    /// Foreign key reference to the associated proficiency level.
    /// </summary>
    public Guid ProficiencyId { get; set; }

    /// <summary>
    /// A collection of localized translations for this keyword record.
    /// </summary>
    public ICollection<KeywordTranslation> Translations { get; init; } = [];

    /// <summary>
    /// Collection of associations between this keyword and projects.
    /// </summary>
    public ICollection<ProjectKeyword> ProjectKeywords { get; init; } = [];

    /// <summary>
    /// Navigation property for the proficiency level associated with this keyword.
    /// </summary>
    public Proficiency Proficiency { get; set; } = null!;
}
