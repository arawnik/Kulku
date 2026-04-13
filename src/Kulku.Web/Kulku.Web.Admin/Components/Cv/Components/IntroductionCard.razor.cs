using Kulku.Application.Cover.Introduction.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class IntroductionCard
{
    [Parameter, EditorRequired]
    public IntroductionTranslationsModel? Introduction { get; set; }

    /// <summary>
    /// Whether this introduction is the currently active one.
    /// </summary>
    [Parameter]
    public bool IsActive { get; set; }

    /// <summary>
    /// Derived status label for display.
    /// </summary>
    [Parameter]
    public string StatusLabel { get; set; } = "Superseded";

    [Parameter]
    public EventCallback<Guid> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmingDelete;

    private string StatusBadgeClass =>
        StatusLabel switch
        {
            "Active" => "bg-success",
            "Scheduled" => "bg-info",
            _ => "bg-secondary",
        };

    private async Task HandleEditClick()
    {
        if (Introduction is not null)
            await OnEdit.InvokeAsync(Introduction.IntroductionId);
    }

    private async Task HandleConfirmDelete()
    {
        if (Introduction is not null)
        {
            _confirmingDelete = false;
            await OnDelete.InvokeAsync(Introduction.IntroductionId);
        }
    }
}
