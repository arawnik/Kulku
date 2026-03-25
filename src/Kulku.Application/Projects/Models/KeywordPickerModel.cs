using Kulku.Domain.Projects;

namespace Kulku.Application.Projects.Models;

/// <summary>
/// Lightweight model for keyword picker controls in edit forms.
/// Contains data to render a grouped checkbox list.
/// </summary>
public sealed record KeywordPickerModel(Guid Id, string Name, KeywordType Type);
