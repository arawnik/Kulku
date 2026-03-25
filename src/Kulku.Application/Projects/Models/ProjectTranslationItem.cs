using Kulku.Domain;

namespace Kulku.Application.Projects.Models;

/// <summary>
/// A single translated name, info, and description entry for a project.
/// </summary>
public sealed record ProjectTranslationItem(
    LanguageCode Language,
    string Name,
    string Info,
    string Description
);
