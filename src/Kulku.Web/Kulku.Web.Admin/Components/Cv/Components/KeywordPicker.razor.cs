using Kulku.Application.Projects.Models;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Cv.Components;

/// <summary>
/// Grouped checkbox picker for selecting keywords.
/// Renders keywords grouped by <see cref="KeywordPickerModel.Type"/> in a responsive grid.
/// </summary>
partial class KeywordPicker
{
    /// <summary>
    /// Available keywords to display in the picker.
    /// </summary>
    [Parameter]
    public IReadOnlyList<KeywordPickerModel>? Keywords { get; set; }

    /// <summary>
    /// The set of currently selected keyword IDs. Mutated in-place on toggle.
    /// </summary>
    [Parameter, EditorRequired]
    public HashSet<Guid> SelectedKeywordIds { get; init; } = [];

    /// <summary>
    /// Disables all checkboxes when <c>true</c>.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    private void ToggleKeyword(Guid keywordId, bool isChecked)
    {
        if (isChecked && !SelectedKeywordIds.Contains(keywordId))
            SelectedKeywordIds.Add(keywordId);
        else if (!isChecked)
            SelectedKeywordIds.Remove(keywordId);
    }
}
