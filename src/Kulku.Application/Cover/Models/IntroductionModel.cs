namespace Kulku.Application.Cover.Models;

/// <summary>
/// Read model for an introduction entry.
/// </summary>
public sealed record IntroductionModel(
    string Title,
    string Content,
    string Tagline,
    string AvatarUrl,
    string SmallAvatarUrl
);
