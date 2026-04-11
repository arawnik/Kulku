using Kulku.Application.Network.Models;
using Kulku.Domain.Network;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Network.Components;

partial class NetworkCompanyCard
{
    [Parameter, EditorRequired]
    public NetworkCompanyModel Company { get; set; } = null!;

    [Parameter]
    public DateTime? LastInteractionDate { get; set; }

    [Parameter]
    public EventCallback<Guid> OnEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDelete { get; set; }

    private bool _confirmDelete;

    internal static string StageBadgeClass(CompanyStage stage) =>
        stage switch
        {
            CompanyStage.Watchlist => "text-bg-secondary",
            CompanyStage.Relationship => "text-bg-info",
            CompanyStage.Discovery => "text-bg-primary",
            CompanyStage.Proposal => "text-bg-warning",
            CompanyStage.ActiveDelivery => "text-bg-success",
            CompanyStage.Parked => "text-bg-accent",
            _ => "text-bg-secondary",
        };

    internal static string StageLabel(CompanyStage stage) =>
        stage switch
        {
            CompanyStage.ActiveDelivery => "Active Delivery",
            _ => stage.ToString(),
        };

    private static string RecencyTextClass(DateTime d)
    {
        var days = (DateTime.Today - d.Date).TotalDays;
        if (days <= 21)
            return "text-success";
        if (days <= 60)
            return "text-warning";
        return "text-danger";
    }
}
