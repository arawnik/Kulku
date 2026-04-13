using Kulku.Domain.Abstractions;
using Kulku.Domain.Network;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="NetworkCategory"/> entities.
/// </summary>
public interface INetworkCategoryRepository : IEntityRepository<NetworkCategory>;
