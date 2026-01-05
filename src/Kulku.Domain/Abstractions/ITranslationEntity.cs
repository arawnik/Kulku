namespace Kulku.Domain.Abstractions;

/// <summary>
/// Marker interface for all translation entities.
/// Used for shared constraints or processing logic.
/// </summary>
public interface ITranslationEntity
{
    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; }
}
