using Kulku.Application.Contacts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Layout;

partial class NavMenu
{
    [Inject]
    private IServiceScopeFactory ScopeFactory { get; set; } = null!;

    private int _newCount;
    private string? currentUrl;

    //TODO: Persist per-user.
    private bool IsCollapsed { get; set; }

    protected override async Task OnInitializedAsync()
    {
        currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;

        // Create a dedicated scope so NavMenu gets its own DbContext instance,
        // avoiding concurrency with the page's OnInitializedAsync.
        await using var scope = ScopeFactory.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<
            IQueryHandler<GetNewContactRequestCount.Query, int>
        >();

        var result = await handler.Handle(new GetNewContactRequestCount.Query(), CancellationToken);

        if (result.IsSuccess)
            _newCount = result.Value;
    }

    private void ToggleCollapsed() => IsCollapsed = !IsCollapsed;

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        InvokeAsync(StateHasChanged);
    }

    protected override void OnDispose() => NavigationManager.LocationChanged -= OnLocationChanged;
}
