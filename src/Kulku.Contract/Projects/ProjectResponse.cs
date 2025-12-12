namespace Kulku.Contract.Projects;

public record ProjectResponse(
    string Name,
    string Info,
    string? Description,
    Uri Url,
    int Order,
    Uri ImageUrl,
    ICollection<KeywordResponse> Keywords
);
