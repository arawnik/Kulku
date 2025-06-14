using Kulku.Contract.Enums;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents the localized content of an <see cref="Experience"/> entry.
/// </summary>
/// <remarks>
/// Used to provide language-specific values for experience.
/// </remarks>
public class ExperienceTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for the translation entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the associated experience.
    /// </summary>
    public Guid ExperienceId { get; set; }

    /// <summary>
    /// Localized title of the experience.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Localized description of the experience.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the parent experience.
    /// </summary>
    public Experience Experience { get; set; } = null!;
}
