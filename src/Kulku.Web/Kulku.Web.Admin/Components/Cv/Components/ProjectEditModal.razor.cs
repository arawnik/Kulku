using System.ComponentModel.DataAnnotations;
using Kulku.Application.Projects.Models;
using Kulku.Domain;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class ProjectEditModal
{
    [Parameter, EditorRequired]
    public ProjectTranslationsModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    /// <summary>
    /// All available keywords for the checkbox picker.
    /// </summary>
    [Parameter]
    public IReadOnlyList<KeywordPickerModel>? Keywords { get; set; }

    [Parameter]
    public EventCallback<ProjectTranslationsModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }

    private ProjectEditFormModel? _form;
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

        if (_lastModelId != Model.ProjectId)
        {
            _lastModelId = Model.ProjectId;
            _form = new ProjectEditFormModel
            {
                ProjectId = Model.ProjectId,
                Url = Model.Url.ToString(),
                ImageUrl = Model.ImageUrl,
                Order = Model.Order,
                SelectedKeywordIds = [.. Model.KeywordIds],
                Translations =
                [
                    .. Model.Translations.Select(t => new TranslationEditModel
                    {
                        Language = t.Language,
                        Name = t.Name,
                        Info = t.Info,
                        Description = t.Description,
                    }),
                ],
            };
            _editContext = new EditContext(_form);
            _editContext.SetFieldCssClassProvider(new BootstrapFieldCssClassProvider());
        }
    }

    private void ToggleKeyword(Guid keywordId, bool isChecked)
    {
        if (_form is null)
            return;

        if (isChecked && !_form.SelectedKeywordIds.Contains(keywordId))
            _form.SelectedKeywordIds.Add(keywordId);
        else if (!isChecked)
            _form.SelectedKeywordIds.Remove(keywordId);
    }

    private async Task HandleSubmit()
    {
        if (_form is null || Model is null || _editContext is null)
            return;
        if (!_editContext.Validate())
            return;

        if (!Uri.TryCreate(_form.Url, UriKind.Absolute, out var parsedUrl))
        {
            _serverValidation?.DisplayErrors(
                [new Error("Url.Invalid", "Please enter a valid URL.", "Url")]
            );
            return;
        }

        var updatedTranslations = _form
            .Translations.Select(t => new ProjectTranslationItem(
                t.Language,
                t.Name,
                t.Info,
                t.Description
            ))
            .ToList();

        var updated = Model with
        {
            Url = parsedUrl,
            ImageUrl = _form.ImageUrl.Trim(),
            Order = _form.Order,
            Translations = updatedTranslations,
            KeywordIds = [.. _form.SelectedKeywordIds],
        };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class ProjectEditFormModel
    {
        public Guid ProjectId { get; init; }

        [Required(ErrorMessage = "Project URL is required.")]
        public string Url { get; set; } = string.Empty;

        [Required(ErrorMessage = "Image filename is required.")]
        public string ImageUrl { get; set; } = string.Empty;

        public int Order { get; set; } = 1;

        public HashSet<Guid> SelectedKeywordIds { get; set; } = [];

        public List<TranslationEditModel> Translations { get; set; } = [];
    }

    private sealed class TranslationEditModel
    {
        public LanguageCode Language { get; init; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Info { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
