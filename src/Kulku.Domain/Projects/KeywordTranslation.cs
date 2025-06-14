using Kulku.Contract.Enums;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a localized version of a keyword, containing language-specific data.
/// </summary>
/// <remarks>
/// Used to provide language-specific values for keyword.
/// </remarks>
public class KeywordTranslation : ITranslationEntity
{
    /// <summary>
    /// Unique identifier for the keyword translation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key reference to the associated keyword.
    /// </summary>
    public Guid KeywordId { get; set; }

    /// <summary>
    /// The localized name of the keyword in the specified language.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Language of the translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// Navigation property to the associated <see cref="Keyword"/> entity.
    /// </summary>
    public Keyword Keyword { get; set; } = null!;
}
