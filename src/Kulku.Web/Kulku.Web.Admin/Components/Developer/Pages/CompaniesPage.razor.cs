using Kulku.Web.Admin.Components.Developer.Components;
using Kulku.Web.Admin.Components.Shared;

namespace Kulku.Web.Admin.Components.Developer.Pages;

partial class CompaniesPage
{
    private enum ViewMode
    {
        List,
        Pipeline,
    }

    private ViewMode _viewMode = ViewMode.List;
    private string _searchText = string.Empty;
    private string _filterStage = string.Empty;
    private Guid _filterCategoryId = Guid.Empty;
    private bool _categoriesExpanded;
    private bool _personalContactsExpanded;

    private IReadOnlyList<CrmCompanyViewModel> _companies = [];
    private IReadOnlyList<CrmCompanyViewModel> _availableCompanies = [];

    private ModalMode? _companyMode;
#pragma warning disable CA2213 // Blazor child component references are managed by the framework
    private CrmCompanyEditModal _companyModal = null!;
#pragma warning restore CA2213

    private ModalMode? _categoryMode;
#pragma warning disable CA2213
    private CrmCategoryEditModal _categoryModal = null!;
#pragma warning restore CA2213

    private ModalMode? _contactMode;
#pragma warning disable CA2213
    private CrmContactEditModal _contactModal = null!;
#pragma warning restore CA2213

    protected override async Task OnInitializedAsync()
    {
        _companies = await Crm.GetEnrolledCompaniesAsync();
    }

    private IReadOnlyList<CrmCompanyViewModel> FilteredCompanies
    {
        get
        {
            var query = _companies.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_searchText))
                query = query.Where(c =>
                    c.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                    || (
                        c.Region?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ?? false
                    )
                );

            if (
                !string.IsNullOrWhiteSpace(_filterStage)
                && Enum.TryParse<CompanyStage>(_filterStage, out var stage)
            )
                query = query.Where(c => c.Profile!.Stage == stage);

            if (_filterCategoryId != Guid.Empty)
                query = query.Where(c => c.Profile!.CategoryIds.Contains(_filterCategoryId));

            return [.. query.OrderBy(c => c.Name)];
        }
    }

    private IReadOnlyList<PipelineColumn.PipelineCompanyItem> GetPipelineItems(CompanyStage stage)
    {
        return
        [
            .. _companies
                .Where(c => c.Profile!.Stage == stage)
                .Select(c =>
                {
                    var interactions = Store.GetCompanyInteractions(c.Id);
                    var categories = c.Profile!.CategoryIds.Select(id => Store.GetCategory(id))
                        .Where(cat => cat is not null)
                        .Cast<CategoryLite>()
                        .ToList();
                    var lastDate = interactions.Select(i => (DateTime?)i.Date).FirstOrDefault();
                    var nextDue = interactions
                        .Where(i => i.NextActionDue.HasValue)
                        .OrderBy(i => i.NextActionDue)
                        .Select(i => i.NextActionDue)
                        .FirstOrDefault();

                    return new PipelineColumn.PipelineCompanyItem(c, categories, lastDate, nextDue);
                })
                .OrderBy(x => x.Company.Name),
        ];
    }

    // ── Company CRUD ──

    private async Task HandleEnrollCompany()
    {
        _availableCompanies = await Crm.GetAvailableCompaniesAsync();
        _companyModal.LoadForCreate();
        _companyMode = ModalMode.Create;
    }

    private void HandleEditCompany(Guid id)
    {
        var company = _companies.FirstOrDefault(c => c.Id == id);
        if (company?.Profile is null)
            return;
        _companyModal.LoadForEdit(company.Profile);
        _companyMode = ModalMode.Edit;
    }

    private async Task HandleSaveCompany(CrmCompanyEditModal.ProfileFormModel form)
    {
        if (_companyMode == ModalMode.Create)
        {
            if (form.IsNewCompany)
            {
                await Crm.CreateAndEnrollAsync(
                    form.NewCompanyName,
                    form.NewCompanyWebsite,
                    form.NewCompanyRegion,
                    form.Stage,
                    form.Notes,
                    [.. form.CategoryIds]
                );
            }
            else
            {
                Crm.EnrollCompany(
                    form.ResolvedCompanyId,
                    form.Stage,
                    form.Notes,
                    [.. form.CategoryIds]
                );
            }
        }
        else
        {
            Store.UpdateProfile(
                form.ResolvedCompanyId,
                form.Stage,
                form.Notes,
                [.. form.CategoryIds]
            );
        }
        CloseCompanyEditor();
        _companies = await Crm.GetEnrolledCompaniesAsync();
    }

    private async Task HandleDeleteCompany(Guid id)
    {
        Store.RemoveProfile(id);
        _companies = await Crm.GetEnrolledCompaniesAsync();
    }

    private void CloseCompanyEditor() => _companyMode = null;

    // ── Category CRUD ──

    private void HandleCreateCategory()
    {
        _categoryModal.LoadForCreate();
        _categoryMode = ModalMode.Create;
    }

    private void HandleEditCategory(CategoryLite category)
    {
        _categoryModal.LoadForEdit(category);
        _categoryMode = ModalMode.Edit;
    }

    private void HandleSaveCategory(CrmCategoryEditModal.CategoryFormModel form)
    {
        if (_categoryMode == ModalMode.Create)
        {
            Store.AddCategory(form.Name, form.ColorToken);
        }
        else if (form.Id.HasValue)
        {
            Store.UpdateCategory(form.Id.Value, form.Name, form.ColorToken);
        }
        CloseCategoryEditor();
    }

    private void HandleDeleteCategory(Guid id)
    {
        Store.RemoveCategory(id);
    }

    private void CloseCategoryEditor() => _categoryMode = null;

    // ── Personal Contacts ──

    private void HandleCreatePersonalContact()
    {
        _contactModal.LoadForCreate();
        _contactMode = ModalMode.Create;
    }

    private async Task HandleSavePersonalContact(CrmContactEditModal.ContactFormModel form)
    {
        if (_contactMode == ModalMode.Create)
        {
            Store.AddContact(
                null,
                form.PersonName,
                form.Email,
                form.Phone,
                form.LinkedInUrl,
                form.Title
            );
        }
        ClosePersonalContactEditor();
        await Task.CompletedTask;
    }

    private void HandleLinkPersonalContact(Guid contactId, Guid companyId)
    {
        Store.MoveContact(contactId, companyId);
    }

    private void HandleDeletePersonalContact(Guid contactId)
    {
        Store.RemoveContact(contactId);
    }

    private void ClosePersonalContactEditor() => _contactMode = null;
}
