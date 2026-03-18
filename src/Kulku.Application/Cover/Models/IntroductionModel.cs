namespace Kulku.Application.Cover.Models;

public sealed record IntroductionModel(
    string Title,
    string Content,
    string Tagline,
    string AvatarUrl,
    string SmallAvatarUrl
);
