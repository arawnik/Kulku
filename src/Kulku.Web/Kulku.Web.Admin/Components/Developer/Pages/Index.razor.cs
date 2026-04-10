namespace Kulku.Web.Admin.Components.Developer.Pages;

partial class Index
{

    // Network pulse
    private int _totalRelationships;
    private int _activeCount;
    private int _goingColdCount;
    private int _followUpsDue;
    private int _overdueCount;

    // Relationship directory
    private IReadOnlyList<CrmCompanyViewModel> _companies = [];
    private List<RelationshipSummary> _relationships = [];
    private string _searchText = string.Empty;
    private string _filterStage = string.Empty;

    // Attention needed
    private List<AttentionItem> _overdueActions = [];
    private List<AttentionItem> _coldRelationships = [];

    // Activity stream
    private IReadOnlyList<InteractionLite> _streamInteractions = [];
    private Dictionary<Guid, string> _companyNameCache = [];

    protected override async Task OnInitializedAsync()
    {
        _companies = await Crm.GetEnrolledCompaniesAsync();
        _companyNameCache = _companies.ToDictionary(c => c.Id, c => c.Name);

        BuildRelationshipSummaries();
        BuildNetworkPulse();
        BuildAttentionLists();
        await BuildStreamAsync();
    }

    private void BuildRelationshipSummaries()
    {
        _relationships = [];

        foreach (var company in _companies)
        {
            var interactions = Store.GetCompanyInteractions(company.Id);
            var contacts = Store.GetCompanyContacts(company.Id);
            var categories = (company.Profile?.CategoryIds ?? [])
                .Select(id => Store.GetCategory(id))
                .Where(c => c is not null)
                .Cast<CategoryLite>()
                .ToList();

            var lastInteraction = interactions.Count > 0 ? interactions[0] : null;
            var firstInteraction = interactions.Count > 0 ? interactions[^1] : null;

            var daysSince = lastInteraction is not null
                ? (int)(DateTime.Today - lastInteraction.Date.Date).TotalDays
                : (int?)null;

            var health = daysSince switch
            {
                null => RelationshipHealth.NoHistory,
                <= 90 => RelationshipHealth.Fresh,
                <= 180 => RelationshipHealth.Cooling,
                _ => RelationshipHealth.Cold,
            };

            var nextActionInteraction = interactions
                .Where(i => i.NextActionDue.HasValue)
                .OrderBy(i => i.NextActionDue)
                .FirstOrDefault();

            var primaryContact = contacts.Count > 0 ? contacts[0] : null;

            _relationships.Add(new RelationshipSummary(
                CompanyId: company.Id,
                CompanyName: company.Name,
                Region: company.Region,
                Stage: company.Profile!.Stage,
                Categories: categories,
                PrimaryContactName: primaryContact?.PersonName,
                PrimaryContactTitle: primaryContact?.Title,
                FirstInteractionDate: firstInteraction?.Date,
                LastInteractionDate: lastInteraction?.Date,
                LastChannel: lastInteraction?.Channel,
                LastSummary: lastInteraction?.Summary,
                DaysSinceLastTouch: daysSince,
                Health: health,
                NextAction: nextActionInteraction?.NextAction,
                NextActionDue: nextActionInteraction?.NextActionDue,
                InteractionCount: interactions.Count,
                ContactCount: contacts.Count
            ));
        }

        // Sort: coldest first (no history, then cold, then cooling, then fresh)
        _relationships = [.. _relationships
            .OrderByDescending(r => r.Health)
            .ThenByDescending(r => r.DaysSinceLastTouch ?? int.MaxValue)];
    }

    private void BuildNetworkPulse()
    {
        _totalRelationships = _companies.Count;

        _activeCount = _relationships.Count(r =>
            r.LastInteractionDate.HasValue
            && (DateTime.Today - r.LastInteractionDate.Value.Date).TotalDays <= 365);

        _goingColdCount = _relationships.Count(r =>
            r.Health is RelationshipHealth.Cold or RelationshipHealth.NoHistory
            && r.NextActionDue is null);

        _followUpsDue = Store.Interactions.Count(i => i.NextActionDue.HasValue);

        _overdueCount = Store.Interactions.Count(i =>
            i.NextActionDue.HasValue && i.NextActionDue.Value.Date < DateTime.Today);
    }

    private void BuildAttentionLists()
    {
        _overdueActions = [];
        foreach (var i in Store.Interactions
            .Where(i => i.NextActionDue.HasValue && i.NextActionDue.Value.Date < DateTime.Today)
            .OrderBy(i => i.NextActionDue))
        {
            var name = _companyNameCache.GetValueOrDefault(i.CompanyId, "Unknown");
            var daysOverdue = (int)(DateTime.Today - i.NextActionDue!.Value.Date).TotalDays;
            _overdueActions.Add(new AttentionItem(
                i.CompanyId, name, i.NextAction ?? "(missing)",
                i.NextActionDue.Value, daysOverdue));
        }

        _coldRelationships = [];
        foreach (var r in _relationships.Where(r =>
            r.Health is RelationshipHealth.Cold or RelationshipHealth.NoHistory
            && r.NextActionDue is null))
        {
            _coldRelationships.Add(new AttentionItem(
                r.CompanyId, r.CompanyName,
                r.LastInteractionDate.HasValue
                    ? $"Last contact {r.DaysSinceLastTouch} days ago"
                    : "No interactions recorded",
                r.LastInteractionDate,
                r.DaysSinceLastTouch));
        }
    }

    private async Task BuildStreamAsync()
    {
        _streamInteractions =
        [
            .. Store.Interactions
                .OrderByDescending(i => i.Date)
                .Take(10),
        ];

        foreach (var interaction in _streamInteractions)
        {
            if (!_companyNameCache.ContainsKey(interaction.CompanyId))
                _companyNameCache[interaction.CompanyId] = await Crm.GetCompanyNameAsync(
                    interaction.CompanyId);
        }
    }

    private IReadOnlyList<RelationshipSummary> FilteredRelationships
    {
        get
        {
            var query = _relationships.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_searchText))
                query = query.Where(r =>
                    r.CompanyName.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                    || (r.Region?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ?? false)
                    || (r.PrimaryContactName?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ?? false));

            if (!string.IsNullOrWhiteSpace(_filterStage)
                && Enum.TryParse<CompanyStage>(_filterStage, out var stage))
                query = query.Where(r => r.Stage == stage);

            return [.. query];
        }
    }

    private sealed record AttentionItem(
        Guid CompanyId,
        string CompanyName,
        string Detail,
        DateTime? Date,
        int? DaysCount
    );
}
