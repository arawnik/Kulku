using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Shared;

partial class MarkdownView
{
    /// <summary>
    /// Raw Markdown (or plain text) to render as HTML.
    /// </summary>
    [Parameter]
    public string? Content { get; set; }
}
