using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Category;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Contact;
using Kulku.Application.Network.Interaction;
using Kulku.Application.Network.Models;
using Kulku.Domain.Network;
using Microsoft.AspNetCore.Components;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Network.Pages;

partial class LogInteraction
{
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
    private ICommandHandler<CreateNetworkContact.Command, Guid> CreateContactHandler { get; set; } =
        null!;

    [Inject]
    private ICommandHandler<
        CreateNetworkInteraction.Command,
        Guid
    > CreateInteractionHandler { get; set; } = null!;

    [Inject]
    private ICommandHandler<UpdateNetworkProfile.Command> UpdateProfileHandler { get; set; } =
        null!;

    [Inject]
    private ILanguageContext LanguageContext { get; set; } = null!;

    private sealed class LogInteractionModel
    {
        public InteractionDirection Direction { get; set; } = InteractionDirection.Inbound;
        public InteractionChannel Channel { get; set; } = InteractionChannel.CvContactForm;
        public DateTime Date { get; set; } = DateTime.Today;

        public bool IsWarmIntro { get; set; }
        public string? ReferredByRelation { get; set; }
        public string? ReferredByName { get; set; }

        public string? ContactPersonName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactLinkedInUrl { get; set; }
        public string? ContactTitle { get; set; }

        public string Summary { get; set; } = "";

        public string? NextAction { get; set; }
        public DateTime? NextActionDue { get; set; }
    }

    private LogInteractionModel Model = new();

    private IReadOnlyList<NetworkCompanyModel> _enrolledCompanies = [];
    private IReadOnlyList<NetworkCategoryModel> _categories = [];
    private IReadOnlyList<NetworkContactModel> _allContacts = [];
    private IReadOnlyList<NetworkInteractionModel> _companyHistory = [];

    private Guid? SelectedCompanyId;
    private Guid? SelectedContactId;
    private bool HasSelectedCompany => SelectedCompanyId.HasValue;

    private HashSet<Guid> SelectedCompanyCategoryIds = new();

    protected override async Task OnInitializedAsync()
    {
        var lang = LanguageContext.Current;

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
            new GetNetworkContacts.Query(lang),
            default
        );
        _allContacts = contactsResult.IsSuccess ? contactsResult.Value ?? [] : [];
    }

    private void ToggleCompanyCategory(Guid id, bool isChecked)
    {
        if (isChecked)
            SelectedCompanyCategoryIds.Add(id);
        else
            SelectedCompanyCategoryIds.Remove(id);
    }

    private async Task OnCompanyChanged(ChangeEventArgs e)
    {
        SelectedCompanyId = Guid.TryParse(e.Value?.ToString(), out var id) ? id : null;
        SelectedContactId = null;

        if (!HasSelectedCompany)
        {
            SelectedCompanyCategoryIds.Clear();
            _companyHistory = [];
        }
        else
        {
            var company = _enrolledCompanies.FirstOrDefault(c =>
                c.CompanyId == SelectedCompanyId!.Value
            );
            if (company is not null)
                SelectedCompanyCategoryIds = [.. company.Categories.Select(c => c.Id)];

            var historyResult = await InteractionQueries.Handle(
                new GetNetworkInteractions.Query(LanguageContext.Current, SelectedCompanyId!.Value),
                default
            );
            _companyHistory = historyResult.IsSuccess ? historyResult.Value ?? [] : [];
        }
    }

    private void OnContactChanged(ChangeEventArgs e)
    {
        SelectedContactId = Guid.TryParse(e.Value?.ToString(), out var id) ? id : null;

        if (SelectedContactId.HasValue)
        {
            var contact = _allContacts.FirstOrDefault(c => c.Id == SelectedContactId.Value);
            if (contact is not null)
            {
                Model.ContactPersonName = contact.PersonName;
                Model.ContactEmail = contact.Email;
                Model.ContactPhone = contact.Phone;
                Model.ContactLinkedInUrl = contact.LinkedInUrl;
                Model.ContactTitle = contact.Title;
            }
        }
        else
        {
            Model.ContactPersonName = null;
            Model.ContactEmail = null;
            Model.ContactPhone = null;
            Model.ContactLinkedInUrl = null;
            Model.ContactTitle = null;
        }
    }

    private IReadOnlyList<NetworkContactModel> CompanyContacts =>
        HasSelectedCompany
            ? [.. _allContacts.Where(c => c.CompanyId == SelectedCompanyId!.Value)]
            : [];

    private IReadOnlyList<NetworkContactModel> PersonalContacts =>
        [.. _allContacts.Where(c => c.CompanyId is null)];

    private void ResetForm()
    {
        Model = new();
        SelectedCompanyId = null;
        SelectedContactId = null;
        SelectedCompanyCategoryIds.Clear();
        _companyHistory = [];
    }

    private async Task OnSubmit()
    {
        if (!HasSelectedCompany)
            return;

        var companyId = SelectedCompanyId!.Value;

        Guid? contactId = SelectedContactId;
        if (
            !contactId.HasValue
            && (
                !string.IsNullOrWhiteSpace(Model.ContactPersonName)
                || !string.IsNullOrWhiteSpace(Model.ContactEmail)
            )
        )
        {
            var contactResult = await CreateContactHandler.Handle(
                new CreateNetworkContact.Command(
                    companyId,
                    Model.ContactPersonName,
                    Model.ContactEmail,
                    Model.ContactPhone,
                    Model.ContactLinkedInUrl,
                    Model.ContactTitle
                ),
                default
            );

            if (contactResult.IsSuccess)
                contactId = contactResult.Value;
        }

        await CreateInteractionHandler.Handle(
            new CreateNetworkInteraction.Command(
                companyId,
                contactId,
                Model.Date.Date,
                Model.Direction,
                Model.Channel,
                Model.IsWarmIntro,
                Model.IsWarmIntro ? Model.ReferredByName : null,
                Model.IsWarmIntro ? Model.ReferredByRelation : null,
                string.IsNullOrWhiteSpace(Model.Summary)
                    ? "(missing summary)"
                    : Model.Summary.Trim(),
                string.IsNullOrWhiteSpace(Model.NextAction) ? null : Model.NextAction.Trim(),
                Model.NextActionDue?.Date
            ),
            default
        );

        Nav.NavigateTo("/network");
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

    private async Task SaveCompanyCategories()
    {
        if (!HasSelectedCompany)
            return;
        var company = _enrolledCompanies.FirstOrDefault(c =>
            c.CompanyId == SelectedCompanyId!.Value
        );
        if (company is null)
            return;
        await UpdateProfileHandler.Handle(
            new UpdateNetworkProfile.Command(
                company.CompanyId,
                company.Stage,
                company.Notes,
                SelectedCompanyCategoryIds.ToList()
            ),
            default
        );
    }
}
