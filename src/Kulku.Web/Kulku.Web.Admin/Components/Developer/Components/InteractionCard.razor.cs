using Kulku.Application.Network.Models;
using Kulku.Domain.Network;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Components;

partial class InteractionCard
{
    [Parameter, EditorRequired]
    public NetworkInteractionModel Interaction { get; set; } = null!;

    [Parameter]
    public bool ShowCompanyName { get; set; } = true;

    [Parameter]
    public string? CompanyLink { get; set; }

    [Parameter]
    public EventCallback<Guid> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmDelete;

    private static string ChannelLabel(InteractionChannel ch) =>
        ch switch
        {
            InteractionChannel.CvContactForm => "CV contact form",
            InteractionChannel.Email => "Email",
            InteractionChannel.Call => "Call",
            InteractionChannel.LinkedIn => "LinkedIn",
            _ => ch.ToString(),
        };

    private static string DueBadge(DateTime due)
    {
        var days = (due.Date - DateTime.Today).TotalDays;
        if (days < 0)
            return "text-bg-danger";
        if (days <= 7)
            return "text-bg-warning";
        return "text-bg-secondary";
    }
}
