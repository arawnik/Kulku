using Kulku.Domain;

namespace Kulku.Application.Projects;

public sealed record ProficiencyTranslationDto(
    LanguageCode Language,
    string Name,
    string? Description
);
