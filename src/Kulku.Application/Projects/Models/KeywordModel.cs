using Kulku.Domain.Projects;

namespace Kulku.Application.Projects.Models;

public sealed record KeywordModel(
    string Name,
    KeywordType Type,
    ProficiencyModel Proficiency,
    int Order,
    bool Display
);
