using Kulku.Application.Projects.Models;

namespace Kulku.Application.IdeaBank.Models;

/// <summary>
/// Full detail model for a single idea, including notes and tags.
/// </summary>
public sealed record IdeaDetailModel(
    Guid Id,
    string Title,
    string? Summary,
    string? Description,
    Guid StatusId,
    string StatusName,
    string StatusStyle,
    Guid PriorityId,
    string PriorityName,
    string PriorityStyle,
    Guid DomainId,
    string DomainName,
    string DomainIcon,
    IReadOnlyList<IdeaTagModel> Tags,
    IReadOnlyList<KeywordPickerModel> Keywords,
    IReadOnlyList<IdeaNoteModel> Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
