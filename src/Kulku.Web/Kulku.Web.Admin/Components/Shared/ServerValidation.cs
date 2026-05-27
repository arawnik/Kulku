using SoulNETLib.Blazor.Bootstrap;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Shared;

/// <summary>
/// Kulku-specific validation component that wraps <see cref="BootstrapValidation"/>
/// to accept <see cref="Error"/> instances directly from command handlers.
/// </summary>
public sealed class ServerValidation : BootstrapValidation
{
    /// <summary>
    /// Adds server validation errors to the form, converting <see cref="Error"/> instances
    /// to the field-path dictionary format used by the base component.
    /// </summary>
    public void DisplayErrors(IEnumerable<Error> errors)
    {
        var grouped = errors
            .GroupBy(e => e.Field ?? string.Empty)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<string>)[.. g.Select(e => e.Message)]);

        DisplayErrors(grouped);
    }
}
