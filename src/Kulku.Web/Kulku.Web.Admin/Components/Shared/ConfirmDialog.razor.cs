using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Shared;

partial class ConfirmDialog
{
    /// <summary>
    /// Controls whether the dialog is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    /// <summary>
    /// Title shown in the dialog header.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Confirm";

    /// <summary>
    /// Message body displayed in the dialog.
    /// </summary>
    [Parameter, EditorRequired]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Text on the confirm button.
    /// </summary>
    [Parameter]
    public string ButtonText { get; set; } = "Confirm";

    /// <summary>
    /// Bootstrap color class for the confirm button (e.g. "danger", "primary").
    /// </summary>
    [Parameter]
    public string ButtonStyle { get; set; } = "danger";

    /// <summary>
    /// Icon displayed on the confirm button.
    /// </summary>
    [Parameter]
    public IconKind ButtonIcon { get; set; } = IconKind.Check;

    /// <summary>
    /// Fired when the user confirms the action.
    /// </summary>
    [Parameter]
    public EventCallback OnConfirm { get; set; }

    /// <summary>
    /// Fired when the user cancels or closes the dialog.
    /// </summary>
    [Parameter]
    public EventCallback OnCancel { get; set; }

    private async Task HandleConfirm() => await OnConfirm.InvokeAsync();

    private async Task HandleCancel() => await OnCancel.InvokeAsync();
}
