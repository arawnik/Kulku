using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Components;

partial class RelationshipRow
{
    [Parameter, EditorRequired]
    public RelationshipSummary Relationship { get; set; } = null!;

    private string FreshnessClass => Relationship.Health switch
    {
        RelationshipHealth.Fresh => "border-success",
        RelationshipHealth.Cooling => "border-warning",
        RelationshipHealth.Cold => "border-danger",
        RelationshipHealth.NoHistory => "border-secondary",
        _ => "border-secondary",
    };

    private static string RelativeTime(DateTime date)
    {
        var days = (DateTime.Today - date.Date).TotalDays;
        if (days < 1)
            return "today";
        if (days < 2)
            return "yesterday";
        if (days < 30)
            return $"{(int)days}d ago";
        if (days < 365)
        {
            var months = (int)(days / 30);
            return months == 1 ? "1 month ago" : $"{months} months ago";
        }
        var years = (int)(days / 365);
        return years == 1 ? "1 year ago" : $"{years} years ago";
    }

    private static string ChannelLabel(InteractionChannel ch) =>
        ch switch
        {
            InteractionChannel.CvContactForm => "CV form",
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

    private static string Truncate(string text, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";
        return text.Length <= maxLength ? text : string.Concat(text.AsSpan(0, maxLength), "…");
    }
}
