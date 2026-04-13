using Kulku.Application.Projects.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class ProjectCard
{
    [Parameter, EditorRequired]
    public ProjectTranslationsModel? Project { get; set; }

    /// <summary>
    /// Keyword names resolved by the parent page for display as badges.
    /// </summary>
    [Parameter]
    public IReadOnlyList<string> KeywordNames { get; set; } = [];

    [Parameter]
    public EventCallback<Guid> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmingDelete;

    private async Task HandleEditClick()
    {
        if (Project is not null)
        {
            await OnEdit.InvokeAsync(Project.ProjectId);
        }
    }

    private async Task HandleConfirmDelete()
    {
        if (Project is not null)
        {
            _confirmingDelete = false;
            await OnDelete.InvokeAsync(Project.ProjectId);
        }
    }
}
