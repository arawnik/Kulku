namespace Kulku.Application.Cover.Models;

public sealed record IntroductionModel(
    string Title,
    string Content,
    string Tagline,
    Uri AvatarUrl,
    Uri SmallAvatarUrl
);
