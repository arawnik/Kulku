using Kulku.Application.Network.Models;
using Kulku.Domain.Network;
using Kulku.Web.Admin.Components.Shared;
using Kulku.Web.Admin.Resources;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Network.Components;

partial class NetworkCompanyEditModal
{
    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Create;

    [Parameter]
    public IReadOnlyList<NetworkCategoryModel> Categories { get; set; } = [];

    /// <summary>
    /// Companies available for enrollment (not yet in the network). Only used in Create mode.
    /// </summary>
    [Parameter]
    public IReadOnlyList<NetworkAvailableCompanyModel> AvailableCompanies { get; set; } = [];

    [Parameter]
    public EventCallback<ProfileFormModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    public ProfileFormModel Form { get; set; } = new();

    private Guid _companyId;

    private string ModalTitle =>
        Mode switch
        {
            ModalMode.Create when Form.IsNewCompany =>
                NetworkStrings.NetworkCompany_CreateAndEnrollTitle,
            ModalMode.Create => NetworkStrings.NetworkCompany_EnrollTitle,
            _ => NetworkStrings.NetworkCompany_EditProfileTitle,
        };

    private string SubmitLabel =>
        Mode switch
        {
            ModalMode.Create when Form.IsNewCompany =>
                NetworkStrings.NetworkCompany_CreateAndEnrollButton,
            ModalMode.Create => NetworkStrings.NetworkCompany_EnrollButton,
            _ => Strings.SaveChanges,
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

    public void LoadForEdit(NetworkCompanyModel company)
    {
        _companyId = company.CompanyId;
        Form = new ProfileFormModel
        {
            SelectedCompanyId = company.CompanyId,
            Notes = company.Notes,
            Stage = company.Stage,
            CategoryIds = [.. company.Categories.Select(c => c.Id)],
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
