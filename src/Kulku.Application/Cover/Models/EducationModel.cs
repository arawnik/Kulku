namespace Kulku.Application.Cover.Models;

public sealed record EducationModel(
    Guid Id,
    string Title,
    string Description,
    InstitutionModel Institution,
    DateOnly StartDate,
    DateOnly? EndDate
);
