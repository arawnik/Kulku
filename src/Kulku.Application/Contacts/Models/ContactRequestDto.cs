namespace Kulku.Application.Contacts.Models;

/// <summary>
/// Input DTO for a user-submitted contact form.
/// </summary>
public record ContactRequestDto(
    string Name,
    string Email,
    string Subject,
    string Message,
    string CaptchaToken
);
