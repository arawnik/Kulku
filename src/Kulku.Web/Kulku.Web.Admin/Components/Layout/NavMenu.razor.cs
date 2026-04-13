using Kulku.Application.Contacts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Layout;

partial class NavMenu
{
    [Inject]
    private IServiceScopeFactory ScopeFactory { get; set; } = null!;

    [Inject]
    private InboxBadgeNotifier BadgeNotifier { get; set; } = null!;

    private int _newCount;
    private string? currentUrl;

    //TODO: Persist per-user.
    private bool IsCollapsed { get; set; }

    protected override async Task OnInitializedAsync()
    {
        currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
        BadgeNotifier.OnChange += OnBadgeChange;

        await RefreshBadgeAsync();
    }

    private async Task RefreshBadgeAsync()
    {
        await using var scope = ScopeFactory.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<
            IQueryHandler<GetContactRequestCountByStatus.Query, int>
        >();

        var result = await handler.Handle(
            new GetContactRequestCountByStatus.Query(),
            CancellationToken
        );

        if (result.IsSuccess)
            _newCount = result.Value;
    }

    private void OnBadgeChange(object? sender, EventArgs e) =>
        InvokeAsync(async () =>
        {
            await RefreshBadgeAsync();
            StateHasChanged();
        });

    private void ToggleCollapsed() => IsCollapsed = !IsCollapsed;

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        InvokeAsync(StateHasChanged);
    }

    protected override void OnDispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
        BadgeNotifier.OnChange -= OnBadgeChange;
    }
}
