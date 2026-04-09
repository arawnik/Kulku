using Kulku.Web.Admin.Components.Developer.Components;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Pages;

partial class CompanyDetailPage
{
    [Parameter]
    public Guid CompanyId { get; set; }

    private CrmCompanyViewModel? _company;
    private IReadOnlyList<CategoryLite> _companyCategories = [];
    private IReadOnlyList<ContactLite> _contacts = [];
    private IReadOnlyList<InteractionLite> _interactions = [];

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
        _company = await Crm.GetCompanyDetailAsync(CompanyId);
        if (_company is null)
            return;

        _companyCategories =
        [
            .. (_company.Profile?.CategoryIds ?? [])
                .Select(id => Store.GetCategory(id))
                .Where(c => c is not null)
                .Cast<CategoryLite>(),
        ];
        _contacts = Store.GetCompanyContacts(CompanyId);
        _interactions = Store.GetCompanyInteractions(CompanyId);
    }

    // ── Stage ──

    private async Task ChangeStage(CompanyStage stage)
    {
        if (_company?.Profile is null)
            return;
        Store.UpdateProfile(
            _company.Id,
            stage,
            _company.Profile.Notes,
            [.. _company.Profile.CategoryIds]
        );
        await ReloadAsync();
    }

    // ── Company Edit ──

    private void HandleEditCompany()
    {
        if (_company?.Profile is null)
            return;
        _companyModal.LoadForEdit(_company.Profile);
        _companyEditVisible = true;
    }

    private async Task HandleSaveCompany(CrmCompanyEditModal.ProfileFormModel form)
    {
        Store.UpdateProfile(form.ResolvedCompanyId, form.Stage, form.Notes, [.. form.CategoryIds]);
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

    private void HandleEditContact(ContactLite contact)
    {
        _contactModal.LoadForEdit(contact);
        _contactMode = ModalMode.Edit;
    }

    private async Task HandleSaveContact(CrmContactEditModal.ContactFormModel form)
    {
        if (_contactMode == ModalMode.Create)
        {
            Store.AddContact(
                CompanyId,
                form.PersonName,
                form.Email,
                form.Phone,
                form.LinkedInUrl,
                form.Title
            );
        }
        else if (form.Id.HasValue)
        {
            Store.UpdateContact(
                form.Id.Value,
                form.PersonName,
                form.Email,
                form.Phone,
                form.LinkedInUrl,
                form.Title
            );
        }
        CloseContactEditor();
        await ReloadAsync();
    }

    private async Task HandleDeleteContact(Guid contactId)
    {
        Store.RemoveContact(contactId);
        await ReloadAsync();
    }

    private void CloseContactEditor() => _contactMode = null;

    // ── Interaction ──

    private async Task HandleDeleteInteraction(Guid interactionId)
    {
        Store.RemoveInteraction(interactionId);
        await ReloadAsync();
    }
}
