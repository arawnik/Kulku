using Kulku.Application.IdeaBank.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Ideas.Components;

partial class IdeaNoteEntry
{
    [Parameter, EditorRequired]
    public IdeaNoteModel Note { get; set; } = null!;

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmDelete;
}
