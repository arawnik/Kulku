using System.ComponentModel.DataAnnotations;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class ExperienceEditModal
{
    [Parameter, EditorRequired]
    public ExperienceTranslationsModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    /// <summary>
    /// Available companies for the dropdown. Only used in <see cref="ModalMode.Create"/> mode.
    /// </summary>
    [Parameter]
    public IReadOnlyList<CompanyTranslationsModel>? Companies { get; set; }

    [Parameter]
    public EventCallback<ExperienceTranslationsModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    /// <summary>
    /// General error message for non-validation failures (e.g. "Not found", load errors).
    /// </summary>
    [Parameter]
    public string? ErrorMessage { get; set; }

    private ExperienceEditFormModel? _form;
    private EditContext? _editContext;
    private ServerValidation? _serverValidation;

    /// <summary>
    /// Track which model instance we built the form from so we only rebuild
    /// when the parent actually provides a different experience, not on every re-render.
    /// </summary>
    private Guid? _lastModelId;

    /// <summary>
    /// Pushes server validation errors into the form's validation state.
    /// Called by the parent page after a failed save.
    /// </summary>
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

        if (_lastModelId != Model.ExperienceId)
        {
            _lastModelId = Model.ExperienceId;
            _form = new ExperienceEditFormModel
            {
                ExperienceId = Model.ExperienceId,
                CompanyId = Model.CompanyId,
                StartDate = Model.StartDate,
                EndDate = Model.EndDate,
                Translations =
                [
                    .. Model.Translations.Select(t => new TranslationEditModel
                    {
                        Language = t.Language,
                        Title = t.Title,
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

        if (Mode == ModalMode.Create && _form.CompanyId == Guid.Empty)
        {
            _serverValidation?.DisplayErrors(
                [new Error("CompanyId.Required", "Please select a company.", "CompanyId")]
            );
            return;
        }

        var updatedTranslations = _form
            .Translations.Select(t => new ExperienceTranslationItem(
                t.Language,
                t.Title,
                t.Description
            ))
            .ToList();

        var updated = Model with
        {
            CompanyId = _form.CompanyId,
            StartDate = _form.StartDate,
            EndDate = _form.EndDate,
            Translations = updatedTranslations,
        };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class ExperienceEditFormModel
    {
        public Guid ExperienceId { get; init; }
        public Guid CompanyId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public List<TranslationEditModel> Translations { get; set; } = [];
    }

    private sealed class TranslationEditModel
    {
        public LanguageCode Language { get; init; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
