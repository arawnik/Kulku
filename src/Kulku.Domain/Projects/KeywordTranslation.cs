using Kulku.Contract.Enums;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a localized version of a keyword, containing language-specific data.
/// </summary>
public class KeywordTranslation
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
    /// The language code that identifies the language of this translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// The localized name of the keyword in the specified language.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to the associated <see cref="Keyword"/> entity.
    /// </summary>
    public Keyword Keyword { get; set; } = null!;
}
