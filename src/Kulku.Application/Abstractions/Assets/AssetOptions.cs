namespace Kulku.Application.Abstractions.Assets;

/// <summary>
/// Configuration for static asset serving. Bind to the <c>Assets</c> section.
/// </summary>
public sealed class AssetOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "Assets";

    /// <summary>
    /// Local directory containing asset sub-folders (e.g. <c>projects/</c>, <c>introductions/</c>).
    /// Relative paths are resolved against the application content root.
    /// </summary>
    public string? LocalPath { get; set; }

    /// <summary>
    /// External base URL for assets (e.g. <c>https://example.com/static</c>).
    /// Used when <see cref="LocalPath"/> is not configured or the directory does not exist.
    /// </summary>
    public string? BaseUrl { get; set; }
}
