using Kulku.Application.Network.Models;
using Kulku.Domain;

namespace Kulku.Application.Network.Ports;

/// <summary>
/// Read-side port for network company queries.
/// </summary>
public interface INetworkCompanyQueries
{
    /// <summary>
    /// Returns all companies that have a network profile, with category, contact, and interaction counts.
    /// </summary>
    Task<IReadOnlyList<NetworkCompanyModel>> ListEnrolledAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a single enrolled company with full detail, or null if not found.
    /// </summary>
    Task<NetworkCompanyDetailModel?> FindByCompanyIdAsync(
        Guid companyId,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns companies that do NOT yet have a network profile.
    /// </summary>
    Task<IReadOnlyList<NetworkAvailableCompanyModel>> ListAvailableAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
