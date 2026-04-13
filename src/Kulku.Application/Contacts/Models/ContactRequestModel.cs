using Kulku.Domain.Contacts;

namespace Kulku.Application.Contacts.Models;

/// <summary>
/// Read model for a contact request.
/// </summary>
public sealed record ContactRequestModel(
    Guid Id,
    string Name,
    string Email,
    string Subject,
    string Message,
    DateTime Timestamp,
    ContactRequestStatus Status
);
