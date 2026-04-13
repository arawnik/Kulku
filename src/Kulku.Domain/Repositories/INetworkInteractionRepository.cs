using Kulku.Domain.Abstractions;
using Kulku.Domain.Network;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="NetworkInteraction"/> entities.
/// </summary>
public interface INetworkInteractionRepository : IEntityRepository<NetworkInteraction>;
