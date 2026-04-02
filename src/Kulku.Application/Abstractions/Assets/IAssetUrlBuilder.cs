namespace Kulku.Application.Abstractions.Assets;

/// <summary>
/// Builds URLs for static assets (projects, introductions).
/// </summary>
public interface IAssetUrlBuilder
{
    /// <summary>
    /// Returns a URL for an asset in the <c>projects</c> folder.
    /// </summary>
    /// <param name="fileName">File name within the <c>projects</c> asset folder.</param>
    /// <returns>Absolute or root-relative URL to the asset.</returns>
    string Project(string fileName);

    /// <summary>
    /// Returns a URL for an asset in the <c>introductions</c> folder.
    /// </summary>
    /// <param name="fileName">File name within the <c>introductions</c> asset folder.</param>
    /// <returns>Absolute or root-relative URL to the asset.</returns>
    string Introduction(string fileName);
}
