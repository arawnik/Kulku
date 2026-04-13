using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using static Microsoft.AspNetCore.Components.Web.RenderMode;

namespace Kulku.Web.Admin.Components;

partial class App
{
    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    private IComponentRenderMode? PageRenderMode =>
        HttpContext.AcceptsInteractiveRouting() ? InteractiveServer : null;
}
