using System.ComponentModel.DataAnnotations;
using Kulku.Application.Cover.Introduction.Models;
using Kulku.Domain;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class IntroductionEditModal
{
    [Parameter, EditorRequired]
    public IntroductionTranslationsModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    [Parameter]
    public EventCallback<IntroductionTranslationsModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }

    private IntroductionEditFormModel? _form;
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

        if (_lastModelId != Model.IntroductionId)
        {
            _lastModelId = Model.IntroductionId;
            _form = new IntroductionEditFormModel
            {
                IntroductionId = Model.IntroductionId,
                AvatarUrl = Model.AvatarUrl,
                SmallAvatarUrl = Model.SmallAvatarUrl,
                PubDate = Model.PubDate,
                Translations =
                [
                    .. Model.Translations.Select(t => new TranslationEditModel
                    {
                        Language = t.Language,
                        Title = t.Title,
                        Tagline = t.Tagline,
                        Content = t.Content,
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
            .Translations.Select(t => new IntroductionTranslationItem(
                t.Language,
                t.Title,
                t.Tagline,
                t.Content
            ))
            .ToList();

        var updated = Model with
        {
            AvatarUrl = _form.AvatarUrl,
            SmallAvatarUrl = _form.SmallAvatarUrl,
            PubDate = _form.PubDate,
            Translations = updatedTranslations,
        };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class IntroductionEditFormModel
    {
        public Guid IntroductionId { get; init; }

        [Required(ErrorMessage = "Avatar filename is required.")]
        public string AvatarUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Small avatar filename is required.")]
        public string SmallAvatarUrl { get; set; } = string.Empty;

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public List<TranslationEditModel> Translations { get; set; } = [];
    }

    private sealed class TranslationEditModel
    {
        public LanguageCode Language { get; init; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        public string Tagline { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
