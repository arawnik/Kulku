using Kulku.Contract.Enums;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents the localized content of a <see cref="Company"/> entry.
/// </summary>
/// <remarks>
/// Used to provide language-specific values for company.
/// </remarks>
public class CompanyTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for company translation entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key reference to the associated <see cref="Company"/>.
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Localized name of the company.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Localized description of the company.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the parent company.
    /// </summary>
    public Company Company { get; set; } = null!;
}
