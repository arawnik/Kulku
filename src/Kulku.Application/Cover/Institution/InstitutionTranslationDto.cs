using Kulku.Domain;

namespace Kulku.Application.Cover.Institution;

public sealed record InstitutionTranslationDto(
    LanguageCode Language,
    string Name,
    string? Department,
    string Description
);
