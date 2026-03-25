namespace Kulku.Application.Cover.Models;

/// <summary>
/// Read model for an education entry and related institution entry.
/// </summary>
public sealed record EducationModel(
    Guid Id,
    string Title,
    string Description,
    InstitutionModel Institution,
    DateOnly StartDate,
    DateOnly? EndDate
);
