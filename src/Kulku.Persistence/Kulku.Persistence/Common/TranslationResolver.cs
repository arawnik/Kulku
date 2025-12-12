using Kulku.Contract.Enums;
using Kulku.Domain.Abstractions;

namespace Kulku.Persistence.Common;

/// <summary>
/// Provides helper methods for resolving translations from translatable entity collections.
/// </summary>
public static class TranslationResolver
{
    /// <summary>
    /// Retrieves the translation matching the specified language from a collection of translations.
    /// </summary>
    /// <typeparam name="TTranslation">The type of the translation entity.</typeparam>
    /// <param name="translations">The collection of translations to search.</param>
    /// <param name="language">The language to resolve the translation for.</param>
    /// <returns>
    /// The translation matching the given language, or <c>null</c> if no matching translation is found.
    /// </returns>
    public static TTranslation? GetTranslation<TTranslation>(
        this IEnumerable<TTranslation> translations,
        LanguageCode language
    )
        where TTranslation : class, ITranslationEntity =>
        translations.FirstOrDefault(t => t.Language == language);

    /// <summary>
    /// Retrieves the translation for the specified language directly from a translatable entity.
    /// </summary>
    /// <typeparam name="TTranslation">The type of the translation entity.</typeparam>
    /// <param name="entity">The entity containing the translations.</param>
    /// <param name="language">The language to resolve the translation for.</param>
    /// <returns>
    /// The translation matching the given language, or <c>null</c> if no matching translation is found.
    /// </returns>
    public static TTranslation? GetTranslation<TTranslation>(
        this ITranslatableEntity<TTranslation> entity,
        LanguageCode language
    )
        where TTranslation : class, ITranslationEntity =>
        entity.Translations.GetTranslation(language);
}
