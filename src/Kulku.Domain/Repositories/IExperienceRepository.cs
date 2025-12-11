using Kulku.Contract.Cover;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Cover;

namespace Kulku.Domain.Repositories;

public interface IExperienceRepository : IEntityRepository<Experience>
{
    Task<ICollection<ExperienceResponse>> QueryAllAsync(
        CancellationToken cancellationToken = default
    );
}
