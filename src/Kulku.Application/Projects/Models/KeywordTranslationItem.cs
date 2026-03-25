using Kulku.Domain;

namespace Kulku.Application.Projects.Models;

/// <summary>
/// A single translated name entry for a keyword.
/// </summary>
public sealed record KeywordTranslationItem(LanguageCode Language, string Name);
