using Kulku.Contract.Projects;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Projects;

namespace Kulku.Domain.Repositories;

public interface IProjectRepository : IEntityRepository<Project>
{
    Task<ICollection<ProjectResponse>> QueryAllAsync(CancellationToken cancellationToken = default);
}
