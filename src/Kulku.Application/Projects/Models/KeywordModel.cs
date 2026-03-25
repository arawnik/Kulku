using Kulku.Domain.Projects;

namespace Kulku.Application.Projects.Models;

/// <summary>
/// Read model for a keyword entry, including its type, associated proficiency level, display order, and visibility flag.
/// </summary>
public sealed record KeywordModel(
    string Name,
    KeywordType Type,
    ProficiencyModel Proficiency,
    int Order,
    bool Display
);
