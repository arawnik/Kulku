namespace Kulku.Application.Network.Models;

/// <summary>
/// Read model for a network contact.
/// </summary>
public sealed record NetworkContactModel(
    Guid Id,
    Guid? CompanyId,
    string? CompanyName,
    string? PersonName,
    string? Email,
    string? Phone,
    string? LinkedInUrl,
    string? Title,
    DateTime CreatedAt
);
