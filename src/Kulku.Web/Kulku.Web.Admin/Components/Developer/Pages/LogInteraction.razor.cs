using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Pages;

partial class LogInteraction
{
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

    private IReadOnlyList<CrmCompanyViewModel> _enrolledCompanies = [];
    private Guid? SelectedCompanyId;
    private Guid? SelectedContactId;
    private bool HasSelectedCompany => SelectedCompanyId.HasValue;

    private HashSet<Guid> SelectedCompanyCategoryIds = new();

    protected override async Task OnInitializedAsync()
    {
        _enrolledCompanies = await Crm.GetEnrolledCompaniesAsync();
    }

    private void ToggleCompanyCategory(Guid id, bool isChecked)
    {
        if (isChecked)
            SelectedCompanyCategoryIds.Add(id);
        else
            SelectedCompanyCategoryIds.Remove(id);
    }

    private void OnCompanyChanged(ChangeEventArgs e)
    {
        SelectedCompanyId = Guid.TryParse(e.Value?.ToString(), out var id) ? id : null;
        SelectedContactId = null;

        if (!HasSelectedCompany)
        {
            SelectedCompanyCategoryIds.Clear();
        }
        else
        {
            var profile = Store.GetProfile(SelectedCompanyId!.Value);
            if (profile is not null)
                SelectedCompanyCategoryIds = [.. profile.CategoryIds];
        }
    }

    private void OnContactChanged(ChangeEventArgs e)
    {
        SelectedContactId = Guid.TryParse(e.Value?.ToString(), out var id) ? id : null;

        if (SelectedContactId.HasValue)
        {
            var contact = Store.GetContact(SelectedContactId.Value);
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

    private IReadOnlyList<ContactLite> CompanyContacts =>
        HasSelectedCompany ? Store.GetCompanyContacts(SelectedCompanyId!.Value) : [];

    private IReadOnlyList<ContactLite> PersonalContacts =>
        Store.GetUnaffiliatedContacts();

    private void ResetForm()
    {
        Model = new();
        SelectedCompanyId = null;
        SelectedContactId = null;
        SelectedCompanyCategoryIds.Clear();
    }

    private Task OnSubmit()
    {
        if (!HasSelectedCompany)
            return Task.CompletedTask;

        var companyId = SelectedCompanyId!.Value;

        ContactLite? contact = null;
        if (SelectedContactId.HasValue)
        {
            contact = Store.GetContact(SelectedContactId.Value);
        }
        else if (
            !string.IsNullOrWhiteSpace(Model.ContactPersonName)
            || !string.IsNullOrWhiteSpace(Model.ContactEmail)
        )
        {
            contact = Store.AddContact(
                companyId,
                Model.ContactPersonName,
                Model.ContactEmail,
                Model.ContactPhone,
                Model.ContactLinkedInUrl,
                Model.ContactTitle
            );
        }

        var interaction = new InteractionLite(
            Id: Guid.NewGuid(),
            CompanyId: companyId,
            Date: Model.Date.Date,
            Direction: Model.Direction,
            Channel: Model.Channel,
            IsWarmIntro: Model.IsWarmIntro,
            ReferredByName: Model.IsWarmIntro ? Model.ReferredByName : null,
            ReferredByRelation: Model.IsWarmIntro ? Model.ReferredByRelation : null,
            ContactId: contact?.Id,
            Summary: string.IsNullOrWhiteSpace(Model.Summary)
                ? "(missing summary)"
                : Model.Summary.Trim(),
            NextAction: string.IsNullOrWhiteSpace(Model.NextAction)
                ? null
                : Model.NextAction.Trim(),
            NextActionDue: Model.NextActionDue?.Date
        );

        Store.AddInteraction(interaction);

        Nav.NavigateTo("/developer");
        return Task.CompletedTask;
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

    private void SaveCompanyCategories()
    {
        if (!HasSelectedCompany)
            return;
        var profile = Store.GetProfile(SelectedCompanyId!.Value);
        if (profile is null)
            return;
        Store.UpdateProfile(
            profile.CompanyId,
            profile.Stage,
            profile.Notes,
            SelectedCompanyCategoryIds.ToList()
        );
    }

    private string? GetContactName(InteractionLite interaction) =>
        interaction.ContactId.HasValue
            ? Store.GetContact(interaction.ContactId.Value)?.PersonName
            : null;
}
