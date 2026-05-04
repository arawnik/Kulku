using Kulku.Domain;
using Kulku.Domain.Abstractions;

namespace Kulku.Application.Cover.Company.Models;

/// <summary>
/// A single translated entry for a company.
/// </summary>
public sealed record CompanyTranslationDto(LanguageCode Language, string Name, string Description)
    : ITranslationDto;
