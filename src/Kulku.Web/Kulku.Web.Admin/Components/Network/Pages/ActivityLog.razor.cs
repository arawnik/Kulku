using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Interaction;
using Kulku.Application.Network.Models;
using Kulku.Domain.Network;
using Microsoft.AspNetCore.Components;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Network.Pages;

partial class ActivityLog
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
    private ILanguageContext LanguageContext { get; set; } = null!;

    private const int PageSize = 20;

    private IReadOnlyList<NetworkCompanyModel> _companies = [];
    private IReadOnlyList<NetworkInteractionModel> _allInteractions = [];
    private int _visibleCount = PageSize;

    // Filters
    private string _filterCompanyId = string.Empty;
    private string _filterChannel = string.Empty;
    private string _filterDirection = string.Empty;
    private string _searchText = string.Empty;

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
        _allInteractions = interactionsResult.IsSuccess
            ? [.. (interactionsResult.Value ?? []).OrderByDescending(i => i.Date)]
            : [];
    }

    private IReadOnlyList<NetworkInteractionModel> FilteredInteractions
    {
        get
        {
            var query = _allInteractions.AsEnumerable();

            if (Guid.TryParse(_filterCompanyId, out var companyId) && companyId != Guid.Empty)
                query = query.Where(i => i.CompanyId == companyId);

            if (
                !string.IsNullOrWhiteSpace(_filterChannel)
                && Enum.TryParse<InteractionChannel>(_filterChannel, out var channel)
            )
                query = query.Where(i => i.Channel == channel);

            if (
                !string.IsNullOrWhiteSpace(_filterDirection)
                && Enum.TryParse<InteractionDirection>(_filterDirection, out var direction)
            )
                query = query.Where(i => i.Direction == direction);

            if (!string.IsNullOrWhiteSpace(_searchText))
                query = query.Where(i =>
                    i.Summary.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                    || i.CompanyName.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                );

            return [.. query];
        }
    }

    private void ShowMore() => _visibleCount += PageSize;

    private void ClearFilters()
    {
        _filterCompanyId = string.Empty;
        _filterChannel = string.Empty;
        _filterDirection = string.Empty;
        _searchText = string.Empty;
        _visibleCount = PageSize;
    }

    private static string ChannelLabel(InteractionChannel ch) =>
        ch switch
        {
            InteractionChannel.CvContactForm => "CV contact form",
            InteractionChannel.Email => "Email",
            InteractionChannel.Call => "Call",
            InteractionChannel.LinkedIn => "LinkedIn",
            _ => ch.ToString(),
        };
}
