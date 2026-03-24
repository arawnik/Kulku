using Kulku.Domain;

namespace Kulku.Application.Cover.Models;

/// <summary>
/// A single translated entry for a company.
/// </summary>
public sealed record CompanyTranslationItem(LanguageCode Language, string Name, string Description);
