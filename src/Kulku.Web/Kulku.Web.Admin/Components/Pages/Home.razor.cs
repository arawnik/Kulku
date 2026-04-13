using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Contacts;
using Kulku.Application.Contacts.Models;
using Kulku.Application.Cover.Education;
using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Introduction;
using Kulku.Application.Cover.Models;
using Kulku.Application.IdeaBank;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Interaction;
using Kulku.Application.Network.Models;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Kulku.Domain.Contacts;
using Microsoft.AspNetCore.Components;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Pages;

partial class Home
{
    // ── Injected query handlers ──

    [Inject]
    private IQueryHandler<GetContactRequestCountByStatus.Query, int> InboxCountQuery { get; set; } =
        null!;

    [Inject]
    private IQueryHandler<
        GetContactRequests.Query,
        IReadOnlyList<ContactRequestModel>
    > ContactRequestQuery { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkCompanies.Query,
        IReadOnlyList<NetworkCompanyModel>
    > CompanyQuery { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkInteractions.Query,
        IReadOnlyList<NetworkInteractionModel>
    > InteractionQuery { get; set; } = null!;

    [Inject]
    private IQueryHandler<GetIdeas.Query, IReadOnlyList<IdeaListModel>> IdeaQuery { get; set; } =
        null!;

    [Inject]
    private IQueryHandler<
        GetProjects.Query,
        IReadOnlyList<ProjectModel>
    > ProjectQuery { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetExperiences.Query,
        IReadOnlyList<ExperienceModel>
    > ExperienceQuery { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetEducations.Query,
        IReadOnlyList<EducationModel>
    > EducationQuery { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetKeywordsForPicker.Query,
        IReadOnlyList<KeywordPickerModel>
    > KeywordQuery { get; set; } = null!;

    [Inject]
    private IQueryHandler<GetIntroduction.Query, IntroductionModel?> IntroQuery { get; set; } =
        null!;

    [Inject]
    private ILanguageContext LanguageContext { get; set; } = null!;

    // ── KPI data ──

    private int _inboxNewCount;
    private int _networkCount;
    private int _ideaCount;
    private int _projectCount;

    // ── Section data ──

    private IReadOnlyList<ContactRequestModel> _recentRequests = [];
    private IReadOnlyList<NetworkCompanyModel> _companies = [];
    private IReadOnlyList<NetworkInteractionModel> _allInteractions = [];
    private IReadOnlyList<IdeaListModel> _recentIdeas = [];
    private IntroductionModel? _introduction;
    private int _experienceCount;
    private int _educationCount;
    private int _keywordCount;

    // ── Attention items ──

    private List<AttentionItem> _overdueActions = [];
    private List<AttentionItem> _coldRelationships = [];

    protected override async Task OnInitializedAsync()
    {
        var lang = LanguageContext.Current;
        var ct = CancellationToken;

        // Inbox
        var inboxResult = await InboxCountQuery.Handle(
            new GetContactRequestCountByStatus.Query(),
            ct
        );
        _inboxNewCount = inboxResult.IsSuccess ? inboxResult.Value : 0;

        var requestsResult = await ContactRequestQuery.Handle(
            new GetContactRequests.Query(ContactRequestStatus.New),
            ct
        );
        _recentRequests = requestsResult.IsSuccess ? [.. (requestsResult.Value ?? []).Take(5)] : [];

        // Network
        var companiesResult = await CompanyQuery.Handle(new GetNetworkCompanies.Query(lang), ct);
        _companies = companiesResult.IsSuccess ? companiesResult.Value ?? [] : [];
        _networkCount = _companies.Count;

        var interactionsResult = await InteractionQuery.Handle(
            new GetNetworkInteractions.Query(lang),
            ct
        );
        _allInteractions = interactionsResult.IsSuccess ? interactionsResult.Value ?? [] : [];

        BuildAttentionItems();

        // Ideas
        var ideasResult = await IdeaQuery.Handle(new GetIdeas.Query(lang), ct);
        var allIdeas = ideasResult.IsSuccess ? ideasResult.Value ?? [] : [];
        _ideaCount = allIdeas.Count;
        _recentIdeas = [.. allIdeas.OrderByDescending(i => i.UpdatedAt).Take(5)];

        // CV
        var projectsResult = await ProjectQuery.Handle(new GetProjects.Query(lang), ct);
        _projectCount = projectsResult.IsSuccess ? (projectsResult.Value?.Count ?? 0) : 0;

        var expResult = await ExperienceQuery.Handle(new GetExperiences.Query(lang), ct);
        _experienceCount = expResult.IsSuccess ? (expResult.Value?.Count ?? 0) : 0;

        var eduResult = await EducationQuery.Handle(new GetEducations.Query(lang), ct);
        _educationCount = eduResult.IsSuccess ? (eduResult.Value?.Count ?? 0) : 0;

        var kwResult = await KeywordQuery.Handle(new GetKeywordsForPicker.Query(), ct);
        _keywordCount = kwResult.IsSuccess ? (kwResult.Value?.Count ?? 0) : 0;

        var introResult = await IntroQuery.Handle(new GetIntroduction.Query(lang), ct);
        _introduction = introResult.IsSuccess ? introResult.Value : null;
    }

    private void BuildAttentionItems()
    {
        _overdueActions = [];
        foreach (
            var i in _allInteractions
                .Where(i => i.NextActionDue.HasValue && i.NextActionDue.Value.Date < DateTime.Today)
                .OrderBy(i => i.NextActionDue)
                .Take(5)
        )
        {
            var daysOverdue = (int)(DateTime.Today - i.NextActionDue!.Value.Date).TotalDays;
            _overdueActions.Add(
                new AttentionItem(
                    i.CompanyId,
                    i.CompanyName,
                    i.NextAction ?? "(missing)",
                    daysOverdue
                )
            );
        }

        // Build relationship health from companies + interactions
        _coldRelationships = [];
        foreach (var company in _companies)
        {
            var lastInteraction = _allInteractions
                .Where(i => i.CompanyId == company.CompanyId)
                .OrderByDescending(i => i.Date)
                .FirstOrDefault();

            var daysSince = lastInteraction is not null
                ? (int)(DateTime.Today - lastInteraction.Date.Date).TotalDays
                : (int?)null;

            var isCold = daysSince is null or > 180;

            // Skip companies that already have a follow-up scheduled
            var hasFollowUp = _allInteractions.Any(i =>
                i.CompanyId == company.CompanyId && i.NextActionDue.HasValue
            );

            if (isCold && !hasFollowUp)
            {
                _coldRelationships.Add(
                    new AttentionItem(
                        company.CompanyId,
                        company.Name,
                        daysSince.HasValue
                            ? $"Last contact {daysSince} days ago"
                            : "No interactions recorded",
                        daysSince
                    )
                );
            }
        }

        _coldRelationships = [.. _coldRelationships.Take(5)];
    }

    private sealed record AttentionItem(
        Guid CompanyId,
        string CompanyName,
        string Detail,
        int? DaysCount
    );
}
