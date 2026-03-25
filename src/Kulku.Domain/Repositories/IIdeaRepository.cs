using Kulku.Domain.Abstractions;
using Kulku.Domain.Ideas;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="Idea"/> entities.
/// </summary>
public interface IIdeaRepository : IEntityRepository<Idea>;
