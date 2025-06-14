using Kulku.Contract.Enums;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a localized version of a proficiency entity, containing language-specific data.
/// </summary>
/// <remarks>
/// Used to provide language-specific values for proficiency.
/// </remarks>
public class ProficiencyTranslation : ITranslationEntity
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
    /// Localized name of the proficiency level.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional localized description of the proficiency level.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the parent <see cref="Proficiency"/>.
    /// </summary>
    public Proficiency Proficiency { get; set; } = null!;
}
