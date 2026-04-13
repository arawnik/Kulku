using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Shared;

partial class MarkdownEditor
{
    /// <summary>
    /// The markdown text value (two-way bindable).
    /// </summary>
    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    /// <summary>
    /// Optional label shown above the toggle buttons.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Number of rows for the textarea.
    /// </summary>
    [Parameter]
    public int Rows { get; set; } = 6;

    /// <summary>
    /// Minimum height (in rem) for the preview pane.
    /// </summary>
    [Parameter]
    public int MinHeight { get; set; } = 6;

    /// <summary>
    /// Whether the textarea is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Placeholder text for the textarea.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    private bool _previewing;
}
