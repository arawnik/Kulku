using System.ComponentModel.DataAnnotations;
using Kulku.Application.Projects.Models;
using Kulku.Domain;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class ProficiencyEditModal
{
    [Parameter, EditorRequired]
    public ProficiencyTranslationsModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    [Parameter]
    public EventCallback<ProficiencyTranslationsModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }

    private ProficiencyEditFormModel? _form;
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

        if (_lastModelId != Model.ProficiencyId)
        {
            _lastModelId = Model.ProficiencyId;
            _form = new ProficiencyEditFormModel
            {
                ProficiencyId = Model.ProficiencyId,
                Scale = Model.Scale,
                Order = Model.Order,
                Translations =
                [
                    .. Model.Translations.Select(t => new ProfTranslationEditModel
                    {
                        Language = t.Language,
                        Name = t.Name,
                        Description = t.Description ?? string.Empty,
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
            .Translations.Select(t => new ProficiencyTranslationItem(
                t.Language,
                t.Name,
                t.Description
            ))
            .ToList();

        var updated = Model with
        {
            Scale = _form.Scale,
            Order = _form.Order,
            Translations = updatedTranslations,
        };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class ProficiencyEditFormModel
    {
        public Guid ProficiencyId { get; init; }

        [Range(0, 100, ErrorMessage = "Scale must be between 0 and 100.")]
        public int Scale { get; set; } = 100;

        public int Order { get; set; } = 1;

        public List<ProfTranslationEditModel> Translations { get; set; } = [];
    }

    private sealed class ProfTranslationEditModel
    {
        public LanguageCode Language { get; init; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
