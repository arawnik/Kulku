using Kulku.Contract.Enums;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a localized version of a proficiency entity, containing language-specific data.
/// </summary>
public class ProficiencyTranslation
{
    /// <summary>
    /// Unique identifier for the translation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key to the associated proficiency level.
    /// </summary>
    public Guid ProficiencyId { get; set; }

    /// <summary>
    /// The language code that identifies the language of this translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Localized name of the proficiency level.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional localized description of the proficiency level.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Navigation property to the parent <see cref="Proficiency"/>.
    /// </summary>
    public Proficiency Proficiency { get; set; } = null!;
}
