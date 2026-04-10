using Kulku.Web.Admin.Components.Developer.Components;

namespace Kulku.Web.Admin.Components.Developer.Pages;

partial class ActivityLog
{
    private const int PageSize = 20;

    private IReadOnlyList<CrmCompanyViewModel> _companies = [];
    private Dictionary<Guid, string> _companyNameCache = [];

    private List<InteractionLite> _allInteractions = [];
    private int _visibleCount = PageSize;

    // Filters
    private string _filterCompanyId = string.Empty;
    private string _filterChannel = string.Empty;
    private string _filterDirection = string.Empty;
    private string _searchText = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _companies = await Crm.GetEnrolledCompaniesAsync();
        _companyNameCache = _companies.ToDictionary(c => c.Id, c => c.Name);

        _allInteractions = [.. Store.Interactions.OrderByDescending(i => i.Date)];

        // Fill in names for any companies not in enrolled list
        foreach (var interaction in _allInteractions)
        {
            if (!_companyNameCache.ContainsKey(interaction.CompanyId))
                _companyNameCache[interaction.CompanyId] = await Crm.GetCompanyNameAsync(
                    interaction.CompanyId);
        }
    }

    private IReadOnlyList<InteractionLite> FilteredInteractions
    {
        get
        {
            var query = _allInteractions.AsEnumerable();

            if (Guid.TryParse(_filterCompanyId, out var companyId) && companyId != Guid.Empty)
                query = query.Where(i => i.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(_filterChannel)
                && Enum.TryParse<InteractionChannel>(_filterChannel, out var channel))
                query = query.Where(i => i.Channel == channel);

            if (!string.IsNullOrWhiteSpace(_filterDirection)
                && Enum.TryParse<InteractionDirection>(_filterDirection, out var direction))
                query = query.Where(i => i.Direction == direction);

            if (!string.IsNullOrWhiteSpace(_searchText))
                query = query.Where(i =>
                    i.Summary.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                    || (_companyNameCache.GetValueOrDefault(i.CompanyId, "")
                        .Contains(_searchText, StringComparison.OrdinalIgnoreCase)));

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
