using Kulku.Application.IdeaBank.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Ideas.Components;

partial class IdeaCard
{
    [Parameter, EditorRequired]
    public IdeaListModel Idea { get; set; } = null!;

    [Parameter]
    public EventCallback<IdeaListModel> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmDelete;
}
