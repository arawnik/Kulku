using Kulku.Domain;

namespace Kulku.Application.Abstractions.Localization;

/// <summary>
/// Provides the current request language for localized application behavior.
/// The language is resolved at the application boundary and exposed
/// as a domain <see cref="LanguageCode"/>.
/// </summary>
public interface ILanguageContext
{
    /// <summary>
    /// Gets the language for the current request.
    /// </summary>
    LanguageCode Current { get; }
}
