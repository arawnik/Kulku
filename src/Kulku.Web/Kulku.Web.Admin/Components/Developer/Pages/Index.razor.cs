namespace Kulku.Web.Admin.Components.Developer.Pages;

partial class Index
{
    private static readonly DateTime MixCutoff = DateTime.Today.AddDays(-90);
    private static readonly DateTime StreamCutoff = DateTime.Today.AddDays(-180);

    private IReadOnlyList<CrmCompanyViewModel> _companies = [];
    private int _enrolledCount;
    private int _inboundTotal;
    private int _outboundTotal;
    private int _overdueCount;
    private List<ChannelMixRow> _activeChannelStats = [];
    private List<NextActionRow> _nextActions = [];
    private IReadOnlyList<InteractionLite> _streamInteractions = [];
    private Dictionary<Guid, string> _companyNameCache = [];

    protected override async Task OnInitializedAsync()
    {
        _companies = await Crm.GetEnrolledCompaniesAsync();
        _enrolledCount = _companies.Count;

        _inboundTotal = Store.Interactions.Count(i =>
            i.Direction == InteractionDirection.Inbound && i.Date >= MixCutoff
        );
        _outboundTotal = Store.Interactions.Count(i =>
            i.Direction == InteractionDirection.Outbound && i.Date >= MixCutoff
        );
        _overdueCount = Store.Interactions.Count(i =>
            i.NextActionDue.HasValue && i.NextActionDue.Value.Date < DateTime.Today
        );

        _activeChannelStats =
        [
            .. Enum.GetValues<InteractionChannel>()
                .Select(ch =>
                {
                    var inbound = Store.Interactions.Count(i =>
                        i.Date >= MixCutoff
                        && i.Channel == ch
                        && i.Direction == InteractionDirection.Inbound
                    );
                    var outbound = Store.Interactions.Count(i =>
                        i.Date >= MixCutoff
                        && i.Channel == ch
                        && i.Direction == InteractionDirection.Outbound
                    );
                    var warm = Store.Interactions.Count(i =>
                        i.Date >= MixCutoff
                        && i.Channel == ch
                        && i.Direction == InteractionDirection.Inbound
                        && i.IsWarmIntro
                    );
                    return new ChannelMixRow(
                        ch,
                        ChannelLabel(ch),
                        ChannelNote(ch),
                        inbound,
                        outbound,
                        warm
                    );
                })
                .Where(r => r.Total > 0)
                .OrderByDescending(r => r.Total),
        ];

        _companyNameCache = _companies.ToDictionary(c => c.Id, c => c.Name);

        _nextActions = [];
        foreach (
            var i in Store
                .Interactions.Where(i => i.NextActionDue is not null)
                .OrderBy(i => i.NextActionDue)
                .Take(8)
        )
        {
            var name = _companyNameCache.GetValueOrDefault(i.CompanyId);
            if (name is null)
                name = await Crm.GetCompanyNameAsync(i.CompanyId);
            _nextActions.Add(
                new NextActionRow(
                    name,
                    i.NextAction ?? "(missing)",
                    i.NextActionDue!.Value.Date,
                    i.Direction,
                    i.Channel,
                    i.IsWarmIntro
                )
            );
        }

        _streamInteractions =
        [
            .. Store
                .Interactions.Where(i => i.Date >= StreamCutoff)
                .OrderByDescending(i => i.Date)
                .Take(16),
        ];

        foreach (var interaction in _streamInteractions)
        {
            if (!_companyNameCache.ContainsKey(interaction.CompanyId))
                _companyNameCache[interaction.CompanyId] = await Crm.GetCompanyNameAsync(
                    interaction.CompanyId
                );
        }
    }

    // Channel mix
    private sealed record ChannelMixRow(
        InteractionChannel Channel,
        string Label,
        string Note,
        int Inbound,
        int Outbound,
        int WarmIntros
    )
    {
        public int Total => Inbound + Outbound;
    }

    // Next actions
    private sealed record NextActionRow(
        string CompanyName,
        string Action,
        DateTime Due,
        InteractionDirection Direction,
        InteractionChannel Channel,
        bool IsWarmIntro
    );

    // UI helpers
    private static string ChannelLabel(InteractionChannel ch) =>
        ch switch
        {
            InteractionChannel.CvContactForm => "CV contact form",
            InteractionChannel.Email => "Email",
            InteractionChannel.Call => "Call",
            InteractionChannel.LinkedIn => "LinkedIn",
            _ => ch.ToString(),
        };

    private static string ChannelNote(InteractionChannel ch) =>
        ch switch
        {
            InteractionChannel.CvContactForm => "Always-on funnel; track quality.",
            InteractionChannel.Email => "Continuation of context; good for depth.",
            InteractionChannel.Call => "Strong signal; usually intent.",
            InteractionChannel.LinkedIn => "High noise; stay selective.",
            _ => "",
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
