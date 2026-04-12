using Kulku.Application.Contacts.Models;
using Kulku.Application.Network.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Inbox.Components;

partial class ConvertContactRequestModal
{
    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public ContactRequestModel? Request { get; set; }

    [Parameter]
    public IReadOnlyList<NetworkCompanyModel> EnrolledCompanies { get; set; } = [];

    [Parameter]
    public EventCallback<ConvertFormModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    public ConvertFormModel Form { get; set; } = new();

    private string? _errorMessage;

    private bool IsSubmitDisabled =>
        Form.IsNewCompany
            ? string.IsNullOrWhiteSpace(Form.NewCompanyName)
            : Form.SelectedCompanyId == Guid.Empty;

    public void Load(ContactRequestModel request)
    {
        Request = request;
        Form = new ConvertFormModel { Summary = $"{request.Subject}: {request.Message}" };
        _errorMessage = null;
    }

    public void SetError(string message) => _errorMessage = message;

    private Task HandleSubmit() => OnSave.InvokeAsync(Form);

    public sealed class ConvertFormModel
    {
        public bool IsNewCompany { get; set; }
        public Guid SelectedCompanyId { get; set; } = Guid.Empty;
        public string NewCompanyName { get; set; } = string.Empty;
        public string? NewCompanyWebsite { get; set; }
        public string? NewCompanyRegion { get; set; }
        public string? Summary { get; set; }
    }
}
