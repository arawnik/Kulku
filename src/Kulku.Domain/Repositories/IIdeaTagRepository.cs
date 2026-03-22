using Kulku.Domain.Abstractions;
using Kulku.Domain.Ideas;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="IdeaTag"/> entities.
/// </summary>
public interface IIdeaTagRepository : IEntityRepository<IdeaTag>;
