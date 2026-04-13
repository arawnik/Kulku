using System.ComponentModel.DataAnnotations;
using Kulku.Application.Projects.Models;
using Kulku.Domain;
using Kulku.Domain.Projects;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Cv.Components;

partial class KeywordEditModal
{
    [Parameter, EditorRequired]
    public KeywordTranslationsModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    [Parameter]
    public IReadOnlyList<ProficiencyTranslationsModel> Proficiencies { get; set; } = [];

    [Parameter]
    public EventCallback<KeywordTranslationsModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }

    private KeywordEditFormModel? _form;
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

        if (_lastModelId != Model.KeywordId)
        {
            _lastModelId = Model.KeywordId;
            _form = new KeywordEditFormModel
            {
                KeywordId = Model.KeywordId,
                Type = Model.Type,
                ProficiencyId = Model.ProficiencyId,
                Order = Model.Order,
                Display = Model.Display,
                Translations =
                [
                    .. Model.Translations.Select(t => new KwTranslationEditModel
                    {
                        Language = t.Language,
                        Name = t.Name,
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
            .Translations.Select(t => new KeywordTranslationItem(t.Language, t.Name))
            .ToList();

        var profName =
            Proficiencies
                .FirstOrDefault(p => p.ProficiencyId == _form.ProficiencyId)
#pragma warning disable CA1826 // Translations may be empty; FirstOrDefault is safer than indexer
                ?.Translations.FirstOrDefault()
                ?.Name ?? string.Empty;
#pragma warning restore CA1826
        var profScale =
            Proficiencies.FirstOrDefault(p => p.ProficiencyId == _form.ProficiencyId)?.Scale ?? 0;

        var updated = Model with
        {
            Type = _form.Type,
            ProficiencyId = _form.ProficiencyId,
            Order = _form.Order,
            Display = _form.Display,
            ProficiencyName = profName,
            ProficiencyScale = profScale,
            Translations = updatedTranslations,
        };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class KeywordEditFormModel
    {
        public Guid KeywordId { get; init; }

        public KeywordType Type { get; set; } = KeywordType.Skill;

        public Guid ProficiencyId { get; set; }

        public int Order { get; set; } = 1;

        public bool Display { get; set; } = true;

        public List<KwTranslationEditModel> Translations { get; set; } = [];
    }

    private sealed class KwTranslationEditModel
    {
        public LanguageCode Language { get; init; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}
