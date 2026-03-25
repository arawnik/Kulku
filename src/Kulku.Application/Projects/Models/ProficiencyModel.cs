namespace Kulku.Application.Projects.Models;

/// <summary>
/// Read model for a proficiency level, including its name, optional description, numeric scale, and display order.
/// </summary>
public sealed record ProficiencyModel(string Name, string? Description, int Scale, int Order);
