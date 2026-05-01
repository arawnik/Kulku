using Kulku.Application.Contacts.Models;
using Kulku.Domain.Contacts;
using Kulku.Web.Admin.Resources;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Inbox.Components;

partial class ContactRequestCard
{
    [Parameter, EditorRequired]
    public ContactRequestModel Request { get; set; } = null!;

    [Parameter]
    public EventCallback<Guid> OnPromote { get; set; }

    [Parameter]
    public EventCallback<Guid> OnMarkSpam { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDismiss { get; set; }

    private bool _confirmSpam;
    private bool _confirmDismiss;

    private string MessagePreview =>
        Request.Message.Length > 200 ? Request.Message[..200] + "…" : Request.Message;

    private string StatusLabel =>
        Request.Status switch
        {
            ContactRequestStatus.New => InboxStrings.Tab_New,
            ContactRequestStatus.Promoted => InboxStrings.Tab_Promoted,
            ContactRequestStatus.Spam => InboxStrings.Tab_Spam,
            ContactRequestStatus.Dismissed => InboxStrings.Tab_Dismissed,
            _ => Request.Status.ToString(),
        };

    private string StatusBadgeClass =>
        Request.Status switch
        {
            ContactRequestStatus.New => "text-bg-primary",
            ContactRequestStatus.Promoted => "text-bg-success",
            ContactRequestStatus.Spam => "text-bg-warning",
            ContactRequestStatus.Dismissed => "text-bg-secondary",
            _ => "text-bg-light border",
        };

    private void RequestSpamConfirm() => _confirmSpam = true;

    private void RequestDismissConfirm() => _confirmDismiss = true;

    private async Task HandleConfirmSpam()
    {
        _confirmSpam = false;
        await OnMarkSpam.InvokeAsync(Request.Id);
    }

    private async Task HandleConfirmDismiss()
    {
        _confirmDismiss = false;
        await OnDismiss.InvokeAsync(Request.Id);
    }
}
