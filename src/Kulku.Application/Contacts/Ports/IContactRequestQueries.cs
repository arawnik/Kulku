using Kulku.Application.Contacts.Models;
using Kulku.Domain.Contacts;

namespace Kulku.Application.Contacts.Ports;

/// <summary>
/// Read-side port for contact request queries.
/// </summary>
public interface IContactRequestQueries
{
    /// <summary>
    /// Returns all contact requests ordered newest first.
    /// </summary>
    Task<IReadOnlyList<ContactRequestModel>> ListAsync(
        ContactRequestStatus? statusFilter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a single contact request by ID, or null if not found.
    /// </summary>
    Task<ContactRequestModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns the count of contact requests with the specified status.
    /// </summary>
    Task<int> CountByStatusAsync(
        ContactRequestStatus status,
        CancellationToken cancellationToken = default
    );
}
