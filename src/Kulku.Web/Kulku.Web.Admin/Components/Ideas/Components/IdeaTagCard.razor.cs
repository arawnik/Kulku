using Kulku.Application.IdeaBank.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Ideas.Components;

partial class IdeaTagCard
{
    [Parameter, EditorRequired]
    public IdeaTagModel Tag { get; set; } = null!;

    [Parameter]
    public EventCallback<IdeaTagModel> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmDelete;
}
