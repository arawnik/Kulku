using Kulku.Contract.Cover;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Cover;

namespace Kulku.Domain.Repositories;

public interface IEducationRepository : IEntityRepository<Education>
{
    Task<ICollection<EducationResponse>> QueryAllAsync(
        CancellationToken cancellationToken = default
    );
}
