using System.ComponentModel.DataAnnotations;
using Kulku.Application.IdeaBank.Models;
using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Ideas.Components;

partial class IdeaTagEditModal
{
    [Parameter, EditorRequired]
    public IdeaTagModel? Model { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Edit;

    [Parameter]
    public EventCallback<IdeaTagModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }

    private TagEditFormModel? _form;
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
            _form = new TagEditFormModel { Name = Model.Name, ColorHex = Model.ColorHex };
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

        var updated = Model with
        {
            Name = _form.Name,
            ColorHex = string.IsNullOrWhiteSpace(_form.ColorHex) ? null : _form.ColorHex,
        };

        await OnSave.InvokeAsync(updated);
    }

    private sealed class TagEditFormModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string? ColorHex { get; set; }
    }
}
