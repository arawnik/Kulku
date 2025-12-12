using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Cover;

/// <summary>
/// Represents an educational institution.
/// </summary>
/// <remarks>
/// This entity supports multilingual translations via <see cref="InstitutionTranslation"/> records.
/// </remarks>
public class Institution : ITranslatableEntity<InstitutionTranslation>
{
    /// <summary>
    /// Unique identifier for the institution.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// A collection of localized translations for this institution record.
    /// </summary>
    public ICollection<InstitutionTranslation> Translations { get; init; } = [];
}
