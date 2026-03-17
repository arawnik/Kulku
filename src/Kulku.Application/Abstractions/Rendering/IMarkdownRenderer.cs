namespace Kulku.Application.Abstractions.Rendering;

/// <summary>
/// Converts Markdown source text to HTML.
/// </summary>
public interface IMarkdownRenderer
{
    /// <summary>
    /// Renders the given Markdown source to an HTML string.
    /// Returns an empty string when <paramref name="markdown"/> is null or whitespace.
    /// </summary>
    string ToHtml(string? markdown);
}
