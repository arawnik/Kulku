using Kulku.Contract.Cover;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Cover;

namespace Kulku.Domain.Repositories;

public interface IIntroductionRepository : IEntityRepository<Introduction>
{
    Task<IntroductionResponse?> QueryCurrentAsync(CancellationToken cancellationToken = default);
}
