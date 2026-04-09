using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Components;

partial class PipelineColumn
{
    [Parameter, EditorRequired]
    public CompanyStage Stage { get; set; }

    [Parameter]
    public IReadOnlyList<PipelineCompanyItem> Companies { get; set; } = [];

    public sealed record PipelineCompanyItem(
        CrmCompanyViewModel Company,
        IReadOnlyList<CategoryLite> Categories,
        DateTime? LastInteractionDate,
        DateTime? NextActionDue
    );

    private static string RecencyTextClass(DateTime d)
    {
        var days = (DateTime.Today - d.Date).TotalDays;
        if (days <= 21)
            return "text-success";
        if (days <= 60)
            return "text-warning";
        return "text-danger";
    }

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
