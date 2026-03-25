using Kulku.Application.Projects.Models;

namespace Kulku.Application.Cover.Models;

/// <summary>
/// Read model for an experience entry, list of related keyword entries and a related company entry.
/// </summary>
public sealed record ExperienceModel(
    Guid Id,
    string Title,
    string Description,
    CompanyModel Company,
    ICollection<KeywordModel> Keywords,
    DateOnly StartDate,
    DateOnly? EndDate
);
