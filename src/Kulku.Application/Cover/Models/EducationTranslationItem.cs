using Kulku.Domain;

namespace Kulku.Application.Cover.Models;

public sealed record EducationTranslationItem(
    LanguageCode Language,
    string Title,
    string Description
);
