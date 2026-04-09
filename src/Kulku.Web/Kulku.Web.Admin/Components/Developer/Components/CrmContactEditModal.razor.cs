using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Components;

partial class CrmContactEditModal
{
    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Create;

    [Parameter]
    public EventCallback<ContactFormModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    public ContactFormModel Form { get; set; } = new();

    public void LoadForCreate()
    {
        Form = new ContactFormModel();
    }

    public void LoadForEdit(ContactLite contact)
    {
        Form = new ContactFormModel
        {
            Id = contact.Id,
            PersonName = contact.PersonName,
            Email = contact.Email,
            Phone = contact.Phone,
            LinkedInUrl = contact.LinkedInUrl,
            Title = contact.Title,
        };
    }

    private Task HandleSave() => OnSave.InvokeAsync(Form);

    public sealed class ContactFormModel
    {
        public Guid? Id { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? Title { get; set; }
    }
}
