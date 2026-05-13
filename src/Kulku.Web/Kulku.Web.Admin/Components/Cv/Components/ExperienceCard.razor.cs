using Kulku.Application.Cover.Experience.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class ExperienceCard
{
    [Parameter, EditorRequired]
    public ExperienceTranslationsModel? Experience { get; set; }

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
        if (Experience is not null)
        {
            await OnEdit.InvokeAsync(Experience.ExperienceId);
        }
    }

    private async Task HandleConfirmDelete()
    {
        if (Experience is not null)
        {
            _confirmingDelete = false;
            await OnDelete.InvokeAsync(Experience.ExperienceId);
        }
    }
}
