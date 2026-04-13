using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Shared;

partial class ModalDialog
{
    /// <summary>
    /// Controls whether the modal is visible.
    /// </summary>
    [Parameter, EditorRequired]
    public bool Visible { get; set; }

    /// <summary>
    /// Text displayed in the modal header.
    /// </summary>
    [Parameter, EditorRequired]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Callback invoked when the header close button (×) is clicked.
    /// </summary>
    [Parameter, EditorRequired]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// Optional dialog size. Defaults to <see cref="ModalSize.Large"/>.
    /// </summary>
    [Parameter]
    public ModalSize Size { get; set; } = ModalSize.Large;

    /// <summary>
    /// The main body content of the modal.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Optional footer content, typically action buttons.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    private string SizeCssClass =>
        Size switch
        {
            ModalSize.Small => "modal-sm",
            ModalSize.Default => "",
            ModalSize.Large => "modal-lg",
            ModalSize.ExtraLarge => "modal-xl",
            _ => "modal-lg",
        };
}
