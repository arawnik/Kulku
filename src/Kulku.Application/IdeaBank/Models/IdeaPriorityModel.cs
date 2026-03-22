namespace Kulku.Application.IdeaBank.Models;

/// <summary>
/// Model for an idea priority level.
/// </summary>
public sealed record IdeaPriorityModel(Guid Id, string Name, string Style, int Order);
