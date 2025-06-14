namespace Kulku.Contract.Cover;

public record IntroductionResponse(
    string Title,
    string Content,
    string Tagline,
    Uri AvatarUrl,
    Uri SmallAvatarUrl
);
