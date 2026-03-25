namespace Kulku.Application.IdeaBank.Models;

/// <summary>
/// Model for a user-managed idea tag.
/// </summary>
public sealed record IdeaTagModel(Guid Id, string Name, string? ColorHex, int IdeaCount);
