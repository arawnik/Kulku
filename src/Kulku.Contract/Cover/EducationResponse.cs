namespace Kulku.Contract.Cover;

public record EducationResponse(
    Guid Id,
    string Title,
    string Description,
    InstitutionResponse Institution,
    DateOnly StartDate,
    DateOnly? EndDate
);
