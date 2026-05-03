using Kulku.Domain;
using Kulku.Domain.Abstractions;

namespace Kulku.Application.Projects;

/// <summary>
/// Input DTO for keyword translation data in create/update commands.
/// </summary>
public sealed record KeywordTranslationDto(LanguageCode Language, string Name) : ITranslationDto;
