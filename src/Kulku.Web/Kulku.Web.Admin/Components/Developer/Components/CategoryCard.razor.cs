using Kulku.Web.Admin.Components.Developer;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Components;

partial class CategoryCard
{
    [Parameter, EditorRequired]
    public CategoryLite Category { get; set; } = null!;

    [Parameter]
    public EventCallback<CategoryLite> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmDelete;
}
