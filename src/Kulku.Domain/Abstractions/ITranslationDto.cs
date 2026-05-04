namespace Kulku.Domain.Abstractions;

/// <summary>
/// Marker interface for translation DTOs, ensuring a <see cref="LanguageCode"/> is present.
/// Used as a constraint for generic translation related operations.
/// </summary>
public interface ITranslationDto
{
    /// <summary>
    /// Language of the translation.
    /// </summary>
    LanguageCode Language { get; }
}
