using Kulku.Web.Admin.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Developer.Components;

partial class CrmCategoryEditModal
{
    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public ModalMode Mode { get; set; } = ModalMode.Create;

    [Parameter]
    public EventCallback<CategoryFormModel> OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    public CategoryFormModel Form { get; set; } = new();

    public void LoadForCreate()
    {
        Form = new CategoryFormModel();
    }

    public void LoadForEdit(CategoryLite category)
    {
        Form = new CategoryFormModel
        {
            Id = category.Id,
            Name = category.Name,
            ColorToken = category.ColorToken,
        };
    }

    private Task HandleSave() => OnSave.InvokeAsync(Form);

    public sealed class CategoryFormModel
    {
        public Guid? Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; } = string.Empty;

        public string? ColorToken { get; set; }
    }
}
