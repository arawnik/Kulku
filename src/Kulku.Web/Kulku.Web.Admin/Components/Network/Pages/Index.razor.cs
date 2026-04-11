using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Contact;
using Kulku.Application.Network.Interaction;
using Kulku.Application.Network.Models;
using Kulku.Domain.Network;
using Microsoft.AspNetCore.Components;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Network.Pages;

partial class Index
{
    [Inject]
    private IQueryHandler<
        GetNetworkCompanies.Query,
        IReadOnlyList<NetworkCompanyModel>
    > CompanyQueries { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkInteractions.Query,
        IReadOnlyList<NetworkInteractionModel>
    > InteractionQueries { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkContacts.Query,
        IReadOnlyList<NetworkContactModel>
    > ContactQueries { get; set; } = null!;

    [Inject]
    private ILanguageContext LanguageContext { get; set; } = null!;

    // Network pulse
    private int _totalRelationships;
    private int _activeCount;
    private int _goingColdCount;
    private int _followUpsDue;
    private int _overdueCount;

    // Relationship directory
    private IReadOnlyList<NetworkCompanyModel> _companies = [];
    private List<RelationshipSummary> _relationships = [];
    private string _searchText = string.Empty;
    private string _filterStage = string.Empty;

    // Attention needed
    private List<AttentionItem> _overdueActions = [];
    private List<AttentionItem> _coldRelationships = [];

    // Activity stream
    private IReadOnlyList<NetworkInteractionModel> _streamInteractions = [];
    private IReadOnlyList<NetworkInteractionModel> _allInteractions = [];
    private IReadOnlyList<NetworkContactModel> _allContacts = [];

    protected override async Task OnInitializedAsync()
    {
        var lang = LanguageContext.Current;

        var companiesResult = await CompanyQueries.Handle(
            new GetNetworkCompanies.Query(lang),
            default
        );
        _companies = companiesResult.IsSuccess ? companiesResult.Value ?? [] : [];

        var interactionsResult = await InteractionQueries.Handle(
            new GetNetworkInteractions.Query(lang),
            default
        );
        _allInteractions = interactionsResult.IsSuccess ? interactionsResult.Value ?? [] : [];

        var contactsResult = await ContactQueries.Handle(
            new GetNetworkContacts.Query(lang),
            default
        );
        _allContacts = contactsResult.IsSuccess ? contactsResult.Value ?? [] : [];

        BuildRelationshipSummaries();
        BuildNetworkPulse();
        BuildAttentionLists();
        BuildStream();
    }

    private void BuildRelationshipSummaries()
    {
        _relationships = [];

        foreach (var company in _companies)
        {
            var interactions = _allInteractions
                .Where(i => i.CompanyId == company.CompanyId)
                .OrderByDescending(i => i.Date)
                .ToList();

            var contacts = _allContacts.Where(c => c.CompanyId == company.CompanyId).ToList();

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

            _relationships.Add(
                new RelationshipSummary(
                    CompanyId: company.CompanyId,
                    CompanyName: company.Name,
                    Region: company.Region,
                    Stage: company.Stage,
                    Categories: company.Categories,
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
                )
            );
        }

        // Sort: coldest first (no history, then cold, then cooling, then fresh)
        _relationships =
        [
            .. _relationships
                .OrderByDescending(r => r.Health)
                .ThenByDescending(r => r.DaysSinceLastTouch ?? int.MaxValue),
        ];
    }

    private void BuildNetworkPulse()
    {
        _totalRelationships = _companies.Count;

        _activeCount = _relationships.Count(r =>
            r.LastInteractionDate.HasValue
            && (DateTime.Today - r.LastInteractionDate.Value.Date).TotalDays <= 365
        );

        _goingColdCount = _relationships.Count(r =>
            r.Health is RelationshipHealth.Cold or RelationshipHealth.NoHistory
            && r.NextActionDue is null
        );

        _followUpsDue = _allInteractions.Count(i => i.NextActionDue.HasValue);

        _overdueCount = _allInteractions.Count(i =>
            i.NextActionDue.HasValue && i.NextActionDue.Value.Date < DateTime.Today
        );
    }

    private void BuildAttentionLists()
    {
        _overdueActions = [];
        foreach (
            var i in _allInteractions
                .Where(i => i.NextActionDue.HasValue && i.NextActionDue.Value.Date < DateTime.Today)
                .OrderBy(i => i.NextActionDue)
        )
        {
            var daysOverdue = (int)(DateTime.Today - i.NextActionDue!.Value.Date).TotalDays;
            _overdueActions.Add(
                new AttentionItem(
                    i.CompanyId,
                    i.CompanyName,
                    i.NextAction ?? "(missing)",
                    i.NextActionDue.Value,
                    daysOverdue
                )
            );
        }

        _coldRelationships = [];
        foreach (
            var r in _relationships.Where(r =>
                r.Health is RelationshipHealth.Cold or RelationshipHealth.NoHistory
                && r.NextActionDue is null
            )
        )
        {
            _coldRelationships.Add(
                new AttentionItem(
                    r.CompanyId,
                    r.CompanyName,
                    r.LastInteractionDate.HasValue
                        ? $"Last contact {r.DaysSinceLastTouch} days ago"
                        : "No interactions recorded",
                    r.LastInteractionDate,
                    r.DaysSinceLastTouch
                )
            );
        }
    }

    private void BuildStream()
    {
        _streamInteractions = [.. _allInteractions.OrderByDescending(i => i.Date).Take(10)];
    }

    private IReadOnlyList<RelationshipSummary> FilteredRelationships
    {
        get
        {
            var query = _relationships.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_searchText))
                query = query.Where(r =>
                    r.CompanyName.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                    || (
                        r.Region?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ?? false
                    )
                    || (
                        r.PrimaryContactName?.Contains(
                            _searchText,
                            StringComparison.OrdinalIgnoreCase
                        ) ?? false
                    )
                );

            if (
                !string.IsNullOrWhiteSpace(_filterStage)
                && Enum.TryParse<CompanyStage>(_filterStage, out var stage)
            )
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
