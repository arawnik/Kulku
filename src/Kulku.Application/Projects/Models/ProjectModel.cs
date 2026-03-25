namespace Kulku.Application.Projects.Models;

/// <summary>
/// Read model for a project entry, including its localized content, URL, image, display order, and associated keywords.
/// </summary>
public sealed record ProjectModel(
    string Name,
    string Info,
    string? Description,
    Uri Url,
    int Order,
    string ImageUrl,
    IReadOnlyList<KeywordModel> Keywords
);
