using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Kulku.Web.Admin.Components.Layout;

partial class NavMenu
{
    private string? currentUrl;

    //TODO: Persist per-user.
    private bool IsCollapsed { get; set; }

    protected override void OnInitialized()
    {
        currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void ToggleCollapsed() => IsCollapsed = !IsCollapsed;

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    protected override void OnDispose() => NavigationManager.LocationChanged -= OnLocationChanged;
}
