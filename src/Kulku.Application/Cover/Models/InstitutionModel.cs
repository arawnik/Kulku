using Kulku.Domain;

namespace Kulku.Application.Cover.Models;

public sealed record InstitutionModel(string Name, string? Department, string Description);

public sealed record InstitutionTranslationItem(
    LanguageCode Language,
    string Name,
    string? Department,
    string Description
);
