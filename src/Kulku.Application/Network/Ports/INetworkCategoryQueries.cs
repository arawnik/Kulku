using Kulku.Application.Network.Models;

namespace Kulku.Application.Network.Ports;

/// <summary>
/// Read-side port for network category queries.
/// </summary>
public interface INetworkCategoryQueries
{
    /// <summary>
    /// Returns all categories ordered by name.
    /// </summary>
    Task<IReadOnlyList<NetworkCategoryModel>> ListAllAsync(
        CancellationToken cancellationToken = default
    );
}
