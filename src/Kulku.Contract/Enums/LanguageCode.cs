using System.Runtime.Serialization;

namespace Kulku.Contract.Enums;

/// <summary>
/// Represents the supported language codes used for localized content.
/// </summary>
public enum LanguageCode
{
    /// <summary>
    /// English language code.
    /// </summary>
    [EnumMember(Value = "EN")]
    English,

    /// <summary>
    /// Finnish language code.
    /// </summary>
    [EnumMember(Value = "FI")]
    Finnish,
}
