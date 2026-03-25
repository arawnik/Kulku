namespace Kulku.Domain.Ideas;

/// <summary>
/// A user-managed cross-cutting label that can be applied to multiple ideas.
/// </summary>
public class IdeaTag
{
    /// <summary>
    /// Unique identifier for the tag.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name of the tag.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional hex color for UI display (e.g. "#6610f2").
    /// </summary>
    public string? ColorHex { get; set; }

    /// <summary>
    /// Many-to-many join entries linking this tag to ideas.
    /// </summary>
    public ICollection<IdeaIdeaTag> IdeaIdeaTags { get; init; } = [];
}
