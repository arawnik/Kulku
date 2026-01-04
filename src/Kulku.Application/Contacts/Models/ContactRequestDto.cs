namespace Kulku.Application.Contacts.Models;

public record ContactRequestDto(
    string Name,
    string Email,
    string Subject,
    string Message,
    string CaptchaToken
);
