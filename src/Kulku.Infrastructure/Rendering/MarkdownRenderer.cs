using Kulku.Application.Abstractions.Rendering;
using Markdig;

namespace Kulku.Infrastructure.Rendering;

/// <summary>
/// Markdig-based markdown renderer. Raw HTML in source is stripped
/// for safety — only Markdown syntax is converted to HTML output.
/// Registered as a singleton (stateless, thread-safe).
/// </summary>
public sealed class MarkdownRenderer : IMarkdownRenderer
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseSoftlineBreakAsHardlineBreak()
        .DisableHtml()
        .Build();

    /// <inheritdoc />
    public string ToHtml(string? markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return string.Empty;

        return Markdown.ToHtml(markdown, Pipeline).Trim();
    }
}
