using Kulku.Domain.Abstractions;
using Kulku.Domain.Projects;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="Keyword"/> entities.
/// </summary>
public interface IKeywordRepository : IEntityRepository<Keyword> { }
