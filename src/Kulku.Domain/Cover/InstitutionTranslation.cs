using Kulku.Contract.Enums;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents the localized content of an <see cref="Institution"/> entry.
/// </summary>
/// <remarks>
/// Used to provide language-specific values for institution.
/// </remarks>
public class InstitutionTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for this translation entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the associated institution.
    /// </summary>
    public Guid InstitutionId { get; set; }

    /// <summary>
    /// Localized name of the institution.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Localized department within the institution, if applicable.
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// Localized description of the institution.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the parent institution.
    /// </summary>
    public Institution Institution { get; set; } = null!;
}
