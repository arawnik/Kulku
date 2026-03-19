using Kulku.Domain;

namespace Kulku.Application.Cover.Company;

public sealed record CompanyTranslationDto(LanguageCode Language, string Name, string Description);
