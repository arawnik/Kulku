using Kulku.Contract.Enums;

namespace Kulku.Domain.Constants;

/// <summary>
/// Defines application-wide default values for domain-level entities and operations.
/// </summary>
public static class Defaults
{
    /// <summary>
    /// The default language used when no specific info is provided.
    /// </summary>
    public const LanguageCode Language = LanguageCode.English;
    public const string Culture = "en";
    public static readonly string[] SupportedCultures = ["en", "fi"];

    /// <summary>
    /// The default allowed length for standard text fields.
    /// </summary>
    public const int TextFieldLength = 255;

    /// <summary>
    /// The default allowed length for larger text areas.
    /// </summary>
    public const int TextAreaLength = 2000;
}
