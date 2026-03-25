namespace Kulku.Application.Projects.Models;

/// <summary>
/// Read model for a project with all its translations and associated keyword IDs.
/// Used by admin views that display and edit content across all languages.
/// </summary>
public sealed record ProjectTranslationsModel(
    Guid ProjectId,
    Uri Url,
    string ImageUrl,
    int Order,
    IReadOnlyList<ProjectTranslationItem> Translations,
    IReadOnlyList<Guid> KeywordIds
);
