using System.ComponentModel.DataAnnotations;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class CompanyEditModal
{
    [Parameter, EditorRequired]
    public CompanyTranslationsModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    [Parameter]
    public EventCallback<CompanyTranslationsModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }

    private CompanyEditFormModel? _form;
    private EditContext? _editContext;
    private ServerValidation? _serverValidation;
    private Guid? _lastModelId;

    public void SetServerErrors(IEnumerable<Error> errors)
    {
        _serverValidation?.DisplayErrors(errors);
    }

    protected override void OnParametersSet()
    {
        if (Model is null)
        {
            _form = null;
            _editContext = null;
            _lastModelId = null;
            return;
        }

        if (_lastModelId != Model.CompanyId)
        {
            _lastModelId = Model.CompanyId;
            _form = new CompanyEditFormModel
            {
                CompanyId = Model.CompanyId,
                Website = Model.Website ?? string.Empty,
                Region = Model.Region ?? string.Empty,
                Translations =
                [
                    .. Model.Translations.Select(t => new CompanyTransEditModel
                    {
                        Language = t.Language,
                        Name = t.Name,
                        Description = t.Description,
                    }),
                ],
            };
            _editContext = new EditContext(_form);
            _editContext.SetFieldCssClassProvider(new BootstrapFieldCssClassProvider());
        }
    }

    private async Task HandleSubmit()
    {
        if (_form is null || Model is null || _editContext is null)
            return;
        if (!_editContext.Validate())
            return;

        var updatedTranslations = _form
            .Translations.Select(t => new CompanyTranslationItem(t.Language, t.Name, t.Description))
            .ToList();

        var updated = Model with
        {
            Website = string.IsNullOrWhiteSpace(_form.Website) ? null : _form.Website.Trim(),
            Region = string.IsNullOrWhiteSpace(_form.Region) ? null : _form.Region.Trim(),
            Translations = updatedTranslations,
        };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class CompanyEditFormModel
    {
        public Guid CompanyId { get; init; }
        public string Website { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public List<CompanyTransEditModel> Translations { get; set; } = [];
    }

    private sealed class CompanyTransEditModel
    {
        public LanguageCode Language { get; init; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
