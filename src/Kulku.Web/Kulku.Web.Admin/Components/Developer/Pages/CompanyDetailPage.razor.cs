using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Category;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Contact;
using Kulku.Application.Network.Interaction;
using Kulku.Application.Network.Models;
using Kulku.Domain.Network;
using Kulku.Web.Admin.Components.Developer.Components;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Developer.Pages;

partial class CompanyDetailPage
{
    [Parameter]
    public Guid CompanyId { get; set; }

    [Inject]
    private IQueryHandler<
        GetNetworkCompanyDetail.Query,
        NetworkCompanyDetailModel?
    > CompanyDetailQuery { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkCompanies.Query,
        IReadOnlyList<NetworkCompanyModel>
    > CompanyQueries { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkCategories.Query,
        IReadOnlyList<NetworkCategoryModel>
    > CategoryQueries { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkContacts.Query,
        IReadOnlyList<NetworkContactModel>
    > ContactQueries { get; set; } = null!;

    [Inject]
    private IQueryHandler<
        GetNetworkInteractions.Query,
        IReadOnlyList<NetworkInteractionModel>
    > InteractionQueries { get; set; } = null!;

    [Inject]
    private ICommandHandler<UpdateNetworkProfile.Command> UpdateProfileHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<CreateNetworkContact.Command, Guid> CreateContactHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<UpdateNetworkContact.Command> UpdateContactHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<DeleteNetworkContact.Command> DeleteContactHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<MoveNetworkContact.Command> MoveContactHandler { get; set; } = null!;

    [Inject]
    private ICommandHandler<DeleteNetworkInteraction.Command> DeleteInteractionHandler { get; set; } =
        null!;

    [Inject]
    private ILanguageContext LanguageContext { get; set; } = null!;

    private NetworkCompanyDetailModel? _company;
    private IReadOnlyList<NetworkCompanyModel> _enrolledCompanies = [];
    private IReadOnlyList<NetworkCategoryModel> _categories = [];
    private IReadOnlyList<NetworkContactModel> _contacts = [];
    private IReadOnlyList<NetworkInteractionModel> _interactions = [];

    private bool _companyEditVisible;
#pragma warning disable CA2213 // Blazor child component references are managed by the framework
    private CrmCompanyEditModal _companyModal = null!;
#pragma warning restore CA2213

    private ModalMode? _contactMode;
#pragma warning disable CA2213
    private CrmContactEditModal _contactModal = null!;
#pragma warning restore CA2213

    protected override async Task OnInitializedAsync()
    {
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        var lang = LanguageContext.Current;

        var detailResult = await CompanyDetailQuery.Handle(
            new GetNetworkCompanyDetail.Query(CompanyId, lang),
            default
        );
        _company = detailResult.IsSuccess ? detailResult.Value : null;
        if (_company is null)
            return;

        var companiesResult = await CompanyQueries.Handle(
            new GetNetworkCompanies.Query(lang),
            default
        );
        _enrolledCompanies = companiesResult.IsSuccess ? companiesResult.Value ?? [] : [];

        var categoriesResult = await CategoryQueries.Handle(
            new GetNetworkCategories.Query(),
            default
        );
        _categories = categoriesResult.IsSuccess ? categoriesResult.Value ?? [] : [];

        var contactsResult = await ContactQueries.Handle(
            new GetNetworkContacts.Query(lang, CompanyId),
            default
        );
        _contacts = contactsResult.IsSuccess ? contactsResult.Value ?? [] : [];

        var interactionsResult = await InteractionQueries.Handle(
            new GetNetworkInteractions.Query(lang, CompanyId),
            default
        );
        _interactions = interactionsResult.IsSuccess ? interactionsResult.Value ?? [] : [];
    }

    // ── Stage ──

    private async Task ChangeStage(CompanyStage stage)
    {
        if (_company is null)
            return;
        await UpdateProfileHandler.Handle(
            new UpdateNetworkProfile.Command(
                CompanyId,
                stage,
                _company.Notes,
                [.. _company.Categories.Select(c => c.Id)]
            ),
            default
        );
        await ReloadAsync();
    }

    // ── Company Edit ──

    private void HandleEditCompany()
    {
        if (_company is null)
            return;

        var model = new NetworkCompanyModel(
            _company.CompanyId,
            _company.Name,
            _company.Website,
            _company.Region,
            _company.Stage,
            _company.Notes,
            _company.Categories,
            _company.ContactCount,
            _company.InteractionCount
        );
        _companyModal.LoadForEdit(model);
        _companyEditVisible = true;
    }

    private async Task HandleSaveCompany(CrmCompanyEditModal.ProfileFormModel form)
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
        CloseCompanyEditor();
        await ReloadAsync();
    }

    private void CloseCompanyEditor() => _companyEditVisible = false;

    // ── Contact CRUD ──

    private void HandleCreateContact()
    {
        _contactModal.LoadForCreate();
        _contactMode = ModalMode.Create;
    }

    private void HandleEditContact(NetworkContactModel contact)
    {
        _contactModal.LoadForEdit(contact);
        _contactMode = ModalMode.Edit;
    }

    private async Task HandleSaveContact(CrmContactEditModal.ContactFormModel form)
    {
        if (_contactMode == ModalMode.Create)
        {
            await CreateContactHandler.Handle(
                new CreateNetworkContact.Command(
                    CompanyId,
                    form.PersonName,
                    form.Email,
                    form.Phone,
                    form.LinkedInUrl,
                    form.Title
                ),
                default
            );
        }
        else if (form.Id.HasValue)
        {
            await UpdateContactHandler.Handle(
                new UpdateNetworkContact.Command(
                    form.Id.Value,
                    form.PersonName,
                    form.Email,
                    form.Phone,
                    form.LinkedInUrl,
                    form.Title
                ),
                default
            );
        }
        CloseContactEditor();
        await ReloadAsync();
    }

    private async Task HandleDeleteContact(Guid contactId)
    {
        await DeleteContactHandler.Handle(new DeleteNetworkContact.Command(contactId), default);
        await ReloadAsync();
    }

    private void CloseContactEditor() => _contactMode = null;

    // ── Move Contact ──

    private async Task HandleMoveContact(Guid contactId, Guid? targetCompanyId)
    {
        await MoveContactHandler.Handle(
            new MoveNetworkContact.Command(contactId, targetCompanyId),
            default
        );
        await ReloadAsync();
    }

    private async Task HandleUnaffiliateContact(Guid contactId)
    {
        await MoveContactHandler.Handle(new MoveNetworkContact.Command(contactId, null), default);
        await ReloadAsync();
    }

    // ── Interaction ──

    private async Task HandleDeleteInteraction(Guid interactionId)
    {
        await DeleteInteractionHandler.Handle(
            new DeleteNetworkInteraction.Command(interactionId),
            default
        );
        await ReloadAsync();
    }
}
