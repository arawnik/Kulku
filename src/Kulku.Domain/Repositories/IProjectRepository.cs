using Kulku.Domain.Abstractions;
using Kulku.Domain.Projects;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="Project"/> entities.
/// </summary>
public interface IProjectRepository : IEntityRepository<Project>;
