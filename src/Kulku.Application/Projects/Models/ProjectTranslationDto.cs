using Kulku.Domain;
using Kulku.Domain.Abstractions;

namespace Kulku.Application.Projects.Models;

/// <summary>
/// Input DTO for project translation data in create/update commands.
/// </summary>
public sealed record ProjectTranslationDto(
    LanguageCode Language,
    string Name,
    string Info,
    string Description
) : ITranslationDto;
