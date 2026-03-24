namespace Kulku.Application.Cover.Models;

/// <summary>
/// Read model for an institution entry.
/// </summary>
public sealed record InstitutionModel(string Name, string? Department, string Description);
