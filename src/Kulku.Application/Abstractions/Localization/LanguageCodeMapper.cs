using System.Globalization;
using Kulku.Domain;

namespace Kulku.Application.Abstractions.Localization;

/// <summary>
/// Maps external culture information to the application's supported <see cref="LanguageCode"/> values.
/// </summary>
/// <remarks>
/// <para>
/// This utility provides an explicit mapping between <see cref="CultureInfo"/> and the domain-level
/// <see cref="LanguageCode"/> enum. It exists to keep localization policy explicit and independent
/// of framework-specific mechanisms.
/// </para>
/// <para>
/// The mapping is intentionally conservative: only explicitly supported languages are mapped,
/// and all other cultures fall back to the application's default language.
/// </para>
/// </remarks>
public static class LanguageCodeMapper
{
    /// <summary>
    /// Maps a <see cref="CultureInfo"/> instance to a supported <see cref="LanguageCode"/>.
    /// </summary>
    /// <param name="culture">
    /// The culture to map. If <c>null</c>, the application's default language is returned.
    /// </param>
    /// <returns>
    /// A supported <see cref="LanguageCode"/> corresponding to the culture, or the default language
    /// if the culture is not recognized.
    /// </returns>
    public static LanguageCode FromCulture(CultureInfo culture)
    {
        if (culture is null)
            return Defaults.Language;

        // Uses standard 2-letter ISO codes.
        var isoCode = culture.TwoLetterISOLanguageName.ToUpperInvariant();

        return isoCode switch
        {
            "EN" => LanguageCode.English,
            "FI" => LanguageCode.Finnish,
            _ => Defaults.Language,
        };
    }
}
