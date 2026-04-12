using Kulku.Domain.Abstractions;
using Kulku.Domain.Network;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="NetworkContact"/> entities.
/// </summary>
public interface INetworkContactRepository : IEntityRepository<NetworkContact>
{
    /// <summary>
    /// Finds an existing contact by email address, or null if no match.
    /// </summary>
    Task<NetworkContact?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    );
}
