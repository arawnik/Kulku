using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Shared;

/// <summary>
/// A reusable Blazor validator component that pushes server-side <see cref="Error"/> instances
/// into the cascaded <see cref="EditContext"/> via a <see cref="ValidationMessageStore"/>.
/// </summary>
/// <remarks>
/// <para>
/// Place this component inside an <see cref="EditForm"/> alongside (or instead of)
/// <c>DataAnnotationsValidator</c>. After a server round-trip returns validation errors,
/// call <see cref="DisplayErrors"/> to show them on the matching fields.
/// </para>
/// <para>
/// The component automatically clears all server errors when a new validation pass is
/// requested (<see cref="EditContext.OnValidationRequested"/>), so stale server messages
/// never block subsequent client-side validation.
/// </para>
/// <para>Supported field name patterns (via <see cref="Error.Field"/>):</para>
/// <list type="bullet">
///   <item><c>"EndDate"</c> — simple property on the root model.</item>
///   <item><c>"Translations[0].Title"</c> — indexed collection, resolved to the list item.</item>
///   <item><c>"Address.City"</c> — nested object property, resolved to the child object.</item>
///   <item><c>null</c> or <c>""</c> — model-level error, shown by <c>ValidationSummary</c>.</item>
/// </list>
/// </remarks>
public sealed class ServerValidation : ComponentBase, IDisposable
{
    private static readonly char[] PathSeparators = ['.', '['];

    private ValidationMessageStore? _messageStore;

    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(ServerValidation)} requires a cascading parameter "
                    + $"of type {nameof(EditContext)}. "
                    + $"For example, you can use {nameof(ServerValidation)} inside an {nameof(EditForm)}."
            );
        }

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += HandleValidationRequested;
    }

    /// <summary>
    /// Adds server validation errors to the form, resolving field names to the
    /// correct <see cref="FieldIdentifier"/> on the <see cref="EditContext.Model"/>.
    /// </summary>
    public void DisplayErrors(IEnumerable<Error> errors)
    {
        if (CurrentEditContext is null || _messageStore is null)
            return;

        _messageStore.Clear();

        foreach (var error in errors)
        {
            var fieldId = string.IsNullOrEmpty(error.Field)
                ? new FieldIdentifier(CurrentEditContext.Model, string.Empty)
                : ToFieldIdentifier(CurrentEditContext, error.Field);

            _messageStore.Add(fieldId, error.Message);
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    /// <summary>
    /// Clears all server-side validation messages.
    /// </summary>
    public void ClearErrors()
    {
        _messageStore?.Clear();
        CurrentEditContext?.NotifyValidationStateChanged();
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        _messageStore?.Clear();
    }

    private const BindingFlags PropertyFlags =
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

    /// <summary>
    /// Walks a dotted property path (e.g. <c>"Translations[0].Title"</c>) starting from
    /// <see cref="EditContext.Model"/> and returns a <see cref="FieldIdentifier"/> pointing
    /// at the deepest reachable (object, property) pair.
    /// Property names are matched case-insensitively so that server field names
    /// (e.g. <c>"endDate"</c> from <c>nameof</c>) resolve to PascalCase model properties.
    /// </summary>
    private static FieldIdentifier ToFieldIdentifier(EditContext editContext, string propertyPath)
    {
        var obj = editContext.Model;

        while (true)
        {
            var nextTokenEnd = propertyPath.IndexOfAny(PathSeparators);
            if (nextTokenEnd < 0)
            {
                // Final segment — resolve to the actual property name for correct casing.
                var finalProp = obj.GetType().GetProperty(propertyPath, PropertyFlags);
                return new FieldIdentifier(obj, finalProp?.Name ?? propertyPath);
            }

            var nextToken = propertyPath[..nextTokenEnd];
            propertyPath = propertyPath[(nextTokenEnd + 1)..];

            object? newObj;
            if (nextToken.EndsWith(']'))
            {
                nextToken = nextToken[..^1];
                var prop = obj.GetType().GetProperty("Item");
                var indexerType = prop?.GetIndexParameters()[0].ParameterType;

                var indexerValue = indexerType is not null
                    ? Convert.ChangeType(nextToken, indexerType, CultureInfo.InvariantCulture)
                    : null;

                newObj = prop?.GetValue(obj, [indexerValue]);
            }
            else
            {
                var prop = obj.GetType().GetProperty(nextToken, PropertyFlags);
                newObj = prop?.GetValue(obj);
            }

            if (newObj is null)
                return new FieldIdentifier(obj, nextToken);

            obj = newObj;
        }
    }

    public void Dispose()
    {
        if (CurrentEditContext is not null)
        {
            CurrentEditContext.OnValidationRequested -= HandleValidationRequested;
        }
    }
}
