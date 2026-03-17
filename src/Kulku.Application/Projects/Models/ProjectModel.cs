namespace Kulku.Application.Projects.Models;

public sealed record ProjectModel(
    string Name,
    string Info,
    string? Description,
    Uri Url,
    int Order,
    string ImageUrl,
    IReadOnlyList<KeywordModel> Keywords
);
