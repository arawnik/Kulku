namespace Kulku.Contract.Contacts;

public record ContactRequestDto(
    string Name,
    string Email,
    string Subject,
    string Message,
    string CaptchaToken
);
