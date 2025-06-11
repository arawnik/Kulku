using System.Globalization;
using Kulku.Contract.Enums;
using Kulku.Domain.Constants;
using SoulNETLib.Common.Extension;

namespace Kulku.Infrastructure.Helpers;

/// <summary>
/// Provides a utility for mapping the current system culture to the application's supported <see cref="LanguageCode"/>.
/// </summary>
public static class CultureLanguageHelper
{
    /// <summary>
    /// Resolves the current system culture (based on <see cref="CultureInfo.CurrentCulture"/>)
    /// to a supported <see cref="LanguageCode"/> enum value.
    /// </summary>
    /// <returns>
    /// The corresponding <see cref="LanguageCode"/> if recognized.
    /// </returns>
    public static LanguageCode GetCurrentLanguage()
    {
        var isoCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToUpperInvariant();

        return isoCode.TryParseEnumMember<LanguageCode>(out var lang)
            ? (LanguageCode)lang
            : Defaults.Language;
    }
}
