using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents a company entity in the CV domain.
/// </summary>
/// <remarks>
/// This entity supports multilingual translations via <see cref="CompanyTranslation"/> records.
/// </remarks>
public class Company : ITranslatableEntity<CompanyTranslation>
{
    /// <summary>
    /// Unique identifier for the company.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Optional website URL for the company.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Optional geographic region or location of the company.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// A collection of localized translations for this company record.
    /// </summary>
    public ICollection<CompanyTranslation> Translations { get; init; } = [];
}
