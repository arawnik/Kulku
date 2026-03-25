using Microsoft.AspNetCore.Components;
#pragma warning disable CA1716 // Identifiers should not match keywords
namespace Kulku.Web.Admin.Components.Shared;

#pragma warning restore CA1716 // Identifiers should not match keywords

partial class InfoCard : ComponentBase
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Title { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Description.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Description { get; set; } = default!;

    /// <summary>
    /// Gets or sets the badge text.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string BadgeText { get; set; } = default!;

    /// <summary>
    /// Gets or sets the badge count.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public int BadgeCount { get; set; } = default!;

    /// <summary>
    /// Gets or sets the badge color.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string BadgeColor { get; set; } = default!;
}
