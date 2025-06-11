using Kulku.Contract.Enums;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a categorized descriptor.
/// </summary>
public class Keyword
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
    /// Collection of language-specific translations for the keyword name.
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
