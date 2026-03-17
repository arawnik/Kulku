using Kulku.Domain.Projects;

namespace Kulku.Application.Projects.Models;

/// <summary>
/// Lightweight model for the keyword picker in project edit forms.
/// Contains just enough data to render a grouped checkbox list.
/// </summary>
public sealed record KeywordPickerModel(Guid Id, string Name, KeywordType Type);
