using System.ComponentModel.DataAnnotations;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class InstitutionEditModal
{
    [Parameter, EditorRequired]
    public InstitutionTranslationsModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    [Parameter]
    public EventCallback<InstitutionTranslationsModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }

    private InstitutionEditFormModel? _form;
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

        if (_lastModelId != Model.InstitutionId)
        {
            _lastModelId = Model.InstitutionId;
            _form = new InstitutionEditFormModel
            {
                InstitutionId = Model.InstitutionId,
                Translations =
                [
                    .. Model.Translations.Select(t => new InstitutionTransEditModel
                    {
                        Language = t.Language,
                        Name = t.Name,
                        Department = t.Department ?? string.Empty,
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
            .Translations.Select(t => new InstitutionTranslationItem(
                t.Language,
                t.Name,
                string.IsNullOrWhiteSpace(t.Department) ? null : t.Department,
                t.Description
            ))
            .ToList();

        var updated = Model with { Translations = updatedTranslations };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class InstitutionEditFormModel
    {
        public Guid InstitutionId { get; init; }
        public List<InstitutionTransEditModel> Translations { get; set; } = [];
    }

    private sealed class InstitutionTransEditModel
    {
        public LanguageCode Language { get; init; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
