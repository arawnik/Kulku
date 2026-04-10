using Kulku.Application.Network.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Components;

partial class CategoryCard
{
    [Parameter, EditorRequired]
    public NetworkCategoryModel Category { get; set; } = null!;

    [Parameter]
    public EventCallback<NetworkCategoryModel> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmDelete;
}
