using Kulku.Application.Projects.Models;

namespace Kulku.Application.Cover.Models;

public sealed record ExperienceModel(
    Guid Id,
    string Title,
    string Description,
    CompanyModel Company,
    ICollection<KeywordModel> Keywords,
    DateOnly StartDate,
    DateOnly? EndDate
);
