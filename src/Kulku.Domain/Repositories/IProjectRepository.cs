using Kulku.Contract.Projects;
using Kulku.Domain.Projects;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Domain.Repositories;

public interface IProjectRepository : IRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Project project);
    void Remove(Project project);

    Task<ICollection<ProjectResponse>> QueryAllAsync(CancellationToken cancellationToken = default);
}
