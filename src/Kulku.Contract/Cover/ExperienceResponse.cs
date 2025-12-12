using Kulku.Contract.Projects;

namespace Kulku.Contract.Cover;

public record ExperienceResponse(
    Guid Id,
    string Title,
    string Description,
    CompanyResponse Company,
    ICollection<KeywordResponse> Keywords,
    DateOnly StartDate,
    DateOnly? EndDate
);
