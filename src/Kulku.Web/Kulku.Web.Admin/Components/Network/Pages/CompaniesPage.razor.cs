using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover.Company;
using Kulku.Application.Cover.Company.Models;
using Kulku.Application.Network.Category;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Contact;
using Kulku.Application.Network.Interaction;
using Kulku.Application.Network.Models;
using Kulku.Domain.Network;
using Kulku.Web.Admin.Components.Network.Components;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Network.Pages;

partial class CompaniesPage
{
    [Inject]
    private IQueryHandler<
        GetNetworkCompanies.Query,
        IReadOnlyList<NetworkCompanyModel>
    > CompanyQueries { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetAvailableNetworkCompanies.Query,
        IReadOnlyList<NetworkAvailableCompanyModel>
    > AvailableCompanyQueries { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkCategories.Query,
        IReadOnlyList<NetworkCategoryModel>
    > CategoryQueries { get; set; } = null!;

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
    private ICommandHandler<EnrollNetworkCompany.Command, Guid> EnrollHandler { get; set; } = null!;

    [Inject]
    private ICommandHandler<UpdateNetworkProfile.Command> UpdateProfileHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<DisenrollNetworkCompany.Command> DisenrollHandler { get; set; } = null!;

    [Inject]
    private ICommandHandler<
        CreateNetworkCategory.Command,
        Guid
    > CreateCategoryHandler { get; set; } = null!;

    [Inject]
    private ICommandHandler<UpdateNetworkCategory.Command> UpdateCategoryHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<DeleteNetworkCategory.Command> DeleteCategoryHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<CreateNetworkContact.Command, Guid> CreateContactHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<DeleteNetworkContact.Command> DeleteContactHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<MoveNetworkContact.Command> MoveContactHandler { get; set; } = null!;

    [Inject]
    private ICommandHandler<CreateCompany.Command, Guid> CreateCompanyHandler { get; set; } = null!;

    [Inject]
    private ILanguageContext LanguageContext { get; set; } = null!;

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

    private IReadOnlyList<NetworkCompanyModel> _companies = [];
    private IReadOnlyList<NetworkAvailableCompanyModel> _availableCompanies = [];
    private IReadOnlyList<NetworkCategoryModel> _categories = [];
    private IReadOnlyList<NetworkInteractionModel> _interactions = [];
    private IReadOnlyList<NetworkContactModel> _personalContacts = [];

    private ModalMode? _companyMode;
#pragma warning disable CA2213 // Blazor child component references are managed by the framework
    private NetworkCompanyEditModal _companyModal = null!;
#pragma warning restore CA2213

    private ModalMode? _categoryMode;
#pragma warning disable CA2213
    private NetworkCategoryEditModal _categoryModal = null!;
#pragma warning restore CA2213

    private ModalMode? _contactMode;
#pragma warning disable CA2213
    private NetworkContactEditModal _contactModal = null!;
#pragma warning restore CA2213

    protected override async Task OnInitializedAsync()
    {
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        var lang = LanguageContext.Current;

        var companiesResult = await CompanyQueries.Handle(
            new GetNetworkCompanies.Query(lang),
            default
        );
        _companies = companiesResult.IsSuccess ? companiesResult.Value ?? [] : [];

        var categoriesResult = await CategoryQueries.Handle(
            new GetNetworkCategories.Query(),
            default
        );
        _categories = categoriesResult.IsSuccess ? categoriesResult.Value ?? [] : [];

        var interactionsResult = await InteractionQueries.Handle(
            new GetNetworkInteractions.Query(lang),
            default
        );
        _interactions = interactionsResult.IsSuccess ? interactionsResult.Value ?? [] : [];

        var contactsResult = await ContactQueries.Handle(
            new GetNetworkContacts.Query(lang),
            default
        );
        var allContacts = contactsResult.IsSuccess ? contactsResult.Value ?? [] : [];
        _personalContacts = [.. allContacts.Where(c => c.CompanyId is null)];
    }

    private IReadOnlyList<NetworkCompanyModel> FilteredCompanies
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
                query = query.Where(c => c.Stage == stage);

            if (_filterCategoryId != Guid.Empty)
                query = query.Where(c => c.Categories.Any(cat => cat.Id == _filterCategoryId));

            return [.. query.OrderBy(c => c.Name)];
        }
    }

    private IReadOnlyList<PipelineColumn.PipelineCompanyItem> GetPipelineItems(CompanyStage stage)
    {
        return
        [
            .. _companies
                .Where(c => c.Stage == stage)
                .Select(c =>
                {
                    var companyInteractions = _interactions.Where(i => i.CompanyId == c.CompanyId);
                    var lastDate = companyInteractions
                        .Select(i => (DateTime?)i.Date)
                        .FirstOrDefault();
                    var nextDue = companyInteractions
                        .Where(i => i.NextActionDue.HasValue)
                        .OrderBy(i => i.NextActionDue)
                        .Select(i => i.NextActionDue)
                        .FirstOrDefault();

                    return new PipelineColumn.PipelineCompanyItem(c, lastDate, nextDue);
                })
                .OrderBy(x => x.Company.Name),
        ];
    }

    // ── Company CRUD ──

    private async Task HandleEnrollCompany()
    {
        var result = await AvailableCompanyQueries.Handle(
            new GetAvailableNetworkCompanies.Query(LanguageContext.Current),
            default
        );
        _availableCompanies = result.IsSuccess ? result.Value ?? [] : [];
        _companyModal.LoadForCreate();
        _companyMode = ModalMode.Create;
    }

    private void HandleEditCompany(Guid companyId)
    {
        var company = _companies.FirstOrDefault(c => c.CompanyId == companyId);
        if (company is null)
            return;
        _companyModal.LoadForEdit(company);
        _companyMode = ModalMode.Edit;
    }

    private async Task HandleSaveCompany(NetworkCompanyEditModal.ProfileFormModel form)
    {
        if (_companyMode == ModalMode.Create)
        {
            Guid companyId;
            if (form.IsNewCompany)
            {
                var translations = LanguageCodeMapper
                    .SupportedLanguageCodes.Select(lc => new CompanyTranslationDto(
                        lc,
                        form.NewCompanyName,
                        string.Empty
                    ))
                    .ToList();

                var createResult = await CreateCompanyHandler.Handle(
                    new CreateCompany.Command(
                        form.NewCompanyWebsite,
                        form.NewCompanyRegion,
                        translations
                    ),
                    default
                );

                if (!createResult.IsSuccess)
                    return;

                companyId = createResult.Value;
            }
            else
            {
                companyId = form.ResolvedCompanyId;
            }

            await EnrollHandler.Handle(
                new EnrollNetworkCompany.Command(
                    companyId,
                    form.Stage,
                    form.Notes,
                    [.. form.CategoryIds]
                ),
                default
            );
        }
        else
        {
            await UpdateProfileHandler.Handle(
                new UpdateNetworkProfile.Command(
                    form.ResolvedCompanyId,
                    form.Stage,
                    form.Notes,
                    [.. form.CategoryIds]
                ),
                default
            );
        }
        CloseCompanyEditor();
        await ReloadAsync();
    }

    private async Task HandleDeleteCompany(Guid companyId)
    {
        await DisenrollHandler.Handle(new DisenrollNetworkCompany.Command(companyId), default);
        await ReloadAsync();
    }

    private void CloseCompanyEditor() => _companyMode = null;

    // ── Category CRUD ──

    private void HandleCreateCategory()
    {
        _categoryModal.LoadForCreate();
        _categoryMode = ModalMode.Create;
    }

    private void HandleEditCategory(NetworkCategoryModel category)
    {
        _categoryModal.LoadForEdit(category);
        _categoryMode = ModalMode.Edit;
    }

    private async Task HandleSaveCategory(NetworkCategoryEditModal.CategoryFormModel form)
    {
        if (_categoryMode == ModalMode.Create)
        {
            await CreateCategoryHandler.Handle(
                new CreateNetworkCategory.Command(form.Name, form.ColorToken),
                default
            );
        }
        else if (form.Id.HasValue)
        {
            await UpdateCategoryHandler.Handle(
                new UpdateNetworkCategory.Command(form.Id.Value, form.Name, form.ColorToken),
                default
            );
        }
        CloseCategoryEditor();
        await ReloadAsync();
    }

    private async Task HandleDeleteCategory(Guid id)
    {
        await DeleteCategoryHandler.Handle(new DeleteNetworkCategory.Command(id), default);
        await ReloadAsync();
    }

    private void CloseCategoryEditor() => _categoryMode = null;

    // ── Personal Contacts ──

    private void HandleCreatePersonalContact()
    {
        _contactModal.LoadForCreate();
        _contactMode = ModalMode.Create;
    }

    private async Task HandleSavePersonalContact(NetworkContactEditModal.ContactFormModel form)
    {
        if (_contactMode == ModalMode.Create)
        {
            await CreateContactHandler.Handle(
                new CreateNetworkContact.Command(
                    null,
                    form.PersonName,
                    form.Email,
                    form.Phone,
                    form.LinkedInUrl,
                    form.Title
                ),
                default
            );
        }
        ClosePersonalContactEditor();
        await ReloadAsync();
    }

    private async Task HandleLinkPersonalContact(Guid contactId, Guid companyId)
    {
        await MoveContactHandler.Handle(
            new MoveNetworkContact.Command(contactId, companyId),
            default
        );
        await ReloadAsync();
    }

    private async Task HandleDeletePersonalContact(Guid contactId)
    {
        await DeleteContactHandler.Handle(new DeleteNetworkContact.Command(contactId), default);
        await ReloadAsync();
    }

    private void ClosePersonalContactEditor() => _contactMode = null;
}
