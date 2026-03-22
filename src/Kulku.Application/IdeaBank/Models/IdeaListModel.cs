namespace Kulku.Application.IdeaBank.Models;

/// <summary>
/// Lightweight model for idea list display.
/// </summary>
public sealed record IdeaListModel(
    Guid Id,
    string Title,
    string? Summary,
    Guid StatusId,
    string StatusName,
    string StatusStyle,
    Guid PriorityId,
    string PriorityName,
    string PriorityStyle,
    Guid DomainId,
    string DomainName,
    string DomainIcon,
    IReadOnlyList<string> TagNames,
    IReadOnlyList<string> KeywordNames,
    int NoteCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
