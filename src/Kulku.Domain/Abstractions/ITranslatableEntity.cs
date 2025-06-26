namespace Kulku.Domain.Abstractions;

/// <summary>
/// Interface for domain entities that support translations.
/// </summary>
/// <typeparam name="TTranslation">The type of translation entity related to this model.</typeparam>
public interface ITranslatableEntity<TTranslation>
    where TTranslation : class, ITranslationEntity
{
    /// <summary>
    /// A collection of localized translations for <see cref="ITranslatableEntity{TTranslation}"/>.
    /// </summary>
    ICollection<TTranslation> Translations { get; init; }
}
