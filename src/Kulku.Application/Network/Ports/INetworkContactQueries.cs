using Kulku.Application.Network.Models;
using Kulku.Domain;

namespace Kulku.Application.Network.Ports;

/// <summary>
/// Read-side port for network contact queries.
/// </summary>
public interface INetworkContactQueries
{
    /// <summary>
    /// Returns contacts for a specific company, or all contacts when companyId is null.
    /// </summary>
    Task<IReadOnlyList<NetworkContactModel>> ListByCompanyAsync(
        Guid? companyId,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns contacts that are not affiliated with any company.
    /// </summary>
    Task<IReadOnlyList<NetworkContactModel>> ListUnaffiliatedAsync(
        CancellationToken cancellationToken = default
    );
}
