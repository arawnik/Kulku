namespace Kulku.Web.Admin.Components.Layout;

/// <summary>
/// Lightweight notification bridge that lets pages signal the NavMenu
/// to refresh its inbox badge count. Scoped per circuit.
/// </summary>
public sealed class InboxBadgeNotifier
{
    /// <summary>
    /// Raised when the inbox badge count may have changed.
    /// </summary>
    public event EventHandler? OnChange;

    /// <summary>
    /// Signals subscribers that the inbox count should be refreshed.
    /// </summary>
    public void Notify() => OnChange?.Invoke(this, EventArgs.Empty);
}
