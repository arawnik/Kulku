using System.Runtime.Serialization;

namespace Kulku.Contract.Enums;

/// <summary>
/// Defines the classification type of a keyword used to describe a project.
/// </summary>
public enum KeywordType
{
    /// <summary>
    /// Represents a programming language.
    /// </summary>
    [EnumMember(Value = "LA")]
    Language,

    /// <summary>
    /// Represents a general skill or capability.
    /// </summary>
    [EnumMember(Value = "SK")]
    Skill,

    /// <summary>
    /// Represents a specific technology, framework, or tool.
    /// </summary>
    [EnumMember(Value = "TE")]
    Technology,
}
