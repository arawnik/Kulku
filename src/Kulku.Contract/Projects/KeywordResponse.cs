using Kulku.Contract.Enums;

namespace Kulku.Contract.Projects;

public record KeywordResponse(
    string Name,
    KeywordType Type,
    ProficiencyResponse Proficiency,
    int Order,
    bool Display
);
