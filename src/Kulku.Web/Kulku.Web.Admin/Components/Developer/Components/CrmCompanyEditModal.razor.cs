using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Components;

partial class CrmCompanyEditModal
{
    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Create;

    [Parameter]
    public IReadOnlyList<CategoryLite> Categories { get; set; } = [];

    /// <summary>
    /// Companies available for enrollment (not yet in CRM). Only used in Create mode.
    /// </summary>
    [Parameter]
    public IReadOnlyList<CrmCompanyViewModel> AvailableCompanies { get; set; } = [];

    [Parameter]
    public EventCallback<ProfileFormModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    public ProfileFormModel Form { get; set; } = new();

    private Guid _companyId;

    private string ModalTitle =>
        Mode switch
        {
            ModalMode.Create when Form.IsNewCompany => "Create & Enroll Company",
            ModalMode.Create => "Enroll Company",
            _ => "Edit CRM Profile",
        };

    private string SubmitLabel =>
        Mode switch
        {
            ModalMode.Create when Form.IsNewCompany => "Create & Enroll",
            ModalMode.Create => "Enroll",
            _ => "Save changes",
        };

    private bool IsSubmitDisabled =>
        Mode == ModalMode.Create
        && (
            Form.IsNewCompany
                ? string.IsNullOrWhiteSpace(Form.NewCompanyName)
                : Form.SelectedCompanyId == Guid.Empty
        );

    public void LoadForCreate()
    {
        _companyId = Guid.Empty;
        Form = new ProfileFormModel();
    }

    public void LoadForEdit(CrmCompanyProfile profile)
    {
        _companyId = profile.CompanyId;
        Form = new ProfileFormModel
        {
            SelectedCompanyId = profile.CompanyId,
            Notes = profile.Notes,
            Stage = profile.Stage,
            CategoryIds = [.. profile.CategoryIds],
        };
    }

    private void ToggleCategory(Guid id, bool isChecked)
    {
        if (isChecked)
            Form.CategoryIds.Add(id);
        else
            Form.CategoryIds.Remove(id);
    }

    private Task HandleSave()
    {
        Form.ResolvedCompanyId = Mode == ModalMode.Create ? Form.SelectedCompanyId : _companyId;
        return OnSave.InvokeAsync(Form);
    }

    public sealed class ProfileFormModel
    {
        public bool IsNewCompany { get; set; }
        public Guid SelectedCompanyId { get; set; } = Guid.Empty;
        public string NewCompanyName { get; set; } = string.Empty;
        public string? NewCompanyWebsite { get; set; }
        public string? NewCompanyRegion { get; set; }
        public string? Notes { get; set; }
        public CompanyStage Stage { get; set; } = CompanyStage.Watchlist;
#pragma warning disable CA2227 // Mutable form model — setter needed for LoadForEdit
        public HashSet<Guid> CategoryIds { get; set; } = [];
#pragma warning restore CA2227

        /// <summary>Set by HandleSave — the resolved company ID (for enroll-existing and edit modes).</summary>
        public Guid ResolvedCompanyId { get; set; }
    }
}
