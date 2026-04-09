using Kulku.Application.Cover.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class InstitutionCard
{
    [Parameter, EditorRequired]
    public InstitutionTranslationsModel Institution { get; set; } = null!;

    [Parameter]
    public EventCallback<Guid> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmingDelete;

    private async Task HandleConfirmDelete()
    {
        _confirmingDelete = false;
        await OnDelete.InvokeAsync(Institution.InstitutionId);
    }
}
