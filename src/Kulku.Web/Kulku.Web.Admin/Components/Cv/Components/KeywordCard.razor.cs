using Kulku.Application.Projects.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class KeywordCard
{
    [Parameter, EditorRequired]
    public KeywordTranslationsModel Keyword { get; set; } = null!;

    [Parameter]
    public EventCallback<Guid> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmingDelete;

    private async Task HandleEditClick()
    {
        await OnEdit.InvokeAsync(Keyword.KeywordId);
    }

    private async Task HandleConfirmDelete()
    {
        _confirmingDelete = false;
        await OnDelete.InvokeAsync(Keyword.KeywordId);
    }
}
