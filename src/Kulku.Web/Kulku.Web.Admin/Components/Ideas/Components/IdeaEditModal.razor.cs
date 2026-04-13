using System.ComponentModel.DataAnnotations;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.Projects.Models;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Ideas.Components;

partial class IdeaEditModal
{
    [Parameter, EditorRequired]
    public IdeaDetailModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    [Parameter]
    public IReadOnlyList<IdeaDomainModel> Domains { get; set; } = [];

    [Parameter]
    public IReadOnlyList<IdeaStatusModel> Statuses { get; set; } = [];

    [Parameter]
    public IReadOnlyList<IdeaPriorityModel> Priorities { get; set; } = [];

    [Parameter]
    public IReadOnlyList<IdeaTagModel> Tags { get; set; } = [];

    [Parameter]
    public IReadOnlyList<KeywordPickerModel> Keywords { get; set; } = [];

    [Parameter]
    public EventCallback<IdeaDetailModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }

    private IdeaEditFormModel? _form;
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

        if (_lastModelId != Model.Id)
        {
            _lastModelId = Model.Id;
            _form = new IdeaEditFormModel
            {
                Title = Model.Title,
                Summary = Model.Summary ?? string.Empty,
                Description = Model.Description ?? string.Empty,
                StatusId = Model.StatusId,
                PriorityId = Model.PriorityId,
                DomainId = Model.DomainId,
                SelectedTagIds = [.. Model.Tags.Select(t => t.Id)],
                SelectedKeywordIds = [.. Model.Keywords.Select(k => k.Id)],
            };
            _editContext = new EditContext(_form);
            _editContext.SetFieldCssClassProvider(new BootstrapFieldCssClassProvider());
        }
    }

    private void ToggleKeyword(Guid keywordId)
    {
        if (_form is null)
            return;
        if (!_form.SelectedKeywordIds.Remove(keywordId))
            _form.SelectedKeywordIds.Add(keywordId);
    }

    private void ToggleTag(Guid tagId)
    {
        if (_form is null)
            return;
        if (!_form.SelectedTagIds.Remove(tagId))
            _form.SelectedTagIds.Add(tagId);
    }

    private async Task HandleSubmit()
    {
        if (_form is null || Model is null || _editContext is null)
            return;
        if (!_editContext.Validate())
            return;

        var updated = Model with
        {
            Title = _form.Title,
            Summary = string.IsNullOrWhiteSpace(_form.Summary) ? null : _form.Summary,
            Description = string.IsNullOrWhiteSpace(_form.Description) ? null : _form.Description,
            StatusId = _form.StatusId,
            PriorityId = _form.PriorityId,
            DomainId = _form.DomainId,
            Tags = [.. Tags.Where(t => _form.SelectedTagIds.Contains(t.Id))],
            Keywords = [.. Keywords.Where(k => _form.SelectedKeywordIds.Contains(k.Id))],
        };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class IdeaEditFormModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required.")]
        public Guid StatusId { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        public Guid PriorityId { get; set; }

        [Required(ErrorMessage = "Domain is required.")]
        public Guid DomainId { get; set; }

        public List<Guid> SelectedTagIds { get; set; } = [];

        public List<Guid> SelectedKeywordIds { get; set; } = [];
    }
}
