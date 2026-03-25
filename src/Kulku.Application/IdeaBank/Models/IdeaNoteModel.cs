namespace Kulku.Application.IdeaBank.Models;

/// <summary>
/// Model for a timestamped idea note.
/// </summary>
public sealed record IdeaNoteModel(Guid Id, string Content, DateTime CreatedAt);
