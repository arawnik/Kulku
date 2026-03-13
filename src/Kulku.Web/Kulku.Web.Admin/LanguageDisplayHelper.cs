using Kulku.Domain;

namespace Kulku.Web.Admin;

/// <summary>
/// Shared UI helpers for the admin application.
/// </summary>
internal static class LanguageDisplayHelper
{
    /// <summary>
    /// Returns a human-readable display name for the given <see cref="LanguageCode"/>.
    /// </summary>
    public static string DisplayName(LanguageCode code) =>
        code switch
        {
            LanguageCode.English => "English",
            LanguageCode.Finnish => "Suomi",
            _ => code.ToString(),
        };
}
