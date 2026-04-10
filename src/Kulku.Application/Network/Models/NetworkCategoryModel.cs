namespace Kulku.Application.Network.Models;

/// <summary>
/// Read model for a network category.
/// </summary>
public sealed record NetworkCategoryModel(Guid Id, string Name, string? ColorToken);
