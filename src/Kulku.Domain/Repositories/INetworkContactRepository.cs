using Kulku.Domain.Abstractions;
using Kulku.Domain.Network;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="NetworkContact"/> entities.
/// </summary>
public interface INetworkContactRepository : IEntityRepository<NetworkContact>;
