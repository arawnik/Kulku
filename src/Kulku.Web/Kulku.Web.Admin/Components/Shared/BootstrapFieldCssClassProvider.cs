using Microsoft.AspNetCore.Components.Forms;

namespace Kulku.Web.Admin.Components.Shared;

/// <summary>
/// Maps Blazor's built-in validation state to Bootstrap's
/// <c>is-valid</c> / <c>is-invalid</c> CSS classes so that
/// <see cref="InputBase{TValue}"/> components render correctly
/// inside Bootstrap-styled forms.
/// </summary>
public sealed class BootstrapFieldCssClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(
        EditContext editContext,
        in FieldIdentifier fieldIdentifier
    )
    {
        var isInvalid = editContext.GetValidationMessages(fieldIdentifier).Any();

        if (!editContext.IsModified(fieldIdentifier))
            return isInvalid ? "is-invalid" : string.Empty;

        return isInvalid ? "is-invalid" : "is-valid";
    }
}
