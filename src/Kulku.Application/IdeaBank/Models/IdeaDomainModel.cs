namespace Kulku.Application.IdeaBank.Models;

/// <summary>
/// Model for an idea categorization domain.
/// </summary>
public sealed record IdeaDomainModel(Guid Id, string Name, string Icon, int Order);
