namespace Kulku.Application.IdeaBank.Models;

/// <summary>
/// Model for an idea lifecycle status.
/// </summary>
public sealed record IdeaStatusModel(Guid Id, string Name, string Style, int Order);
