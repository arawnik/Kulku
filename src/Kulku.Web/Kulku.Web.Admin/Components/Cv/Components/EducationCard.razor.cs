using Kulku.Application.Cover.Education.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class EducationCard
{
    [Parameter, EditorRequired]
    public EducationTranslationsModel? Education { get; set; }

    [Parameter]
    public EventCallback<Guid> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmingDelete;

    private async Task HandleEditClick()
    {
        if (Education is not null)
        {
            await OnEdit.InvokeAsync(Education.EducationId);
        }
    }

    private async Task HandleConfirmDelete()
    {
        if (Education is not null)
        {
            _confirmingDelete = false;
            await OnDelete.InvokeAsync(Education.EducationId);
        }
    }
}
