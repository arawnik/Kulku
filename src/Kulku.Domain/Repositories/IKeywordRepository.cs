using Kulku.Contract.Enums;
using Kulku.Contract.Projects;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Projects;

namespace Kulku.Domain.Repositories;

public interface IKeywordRepository : IEntityRepository<Keyword>
{
    Task<KeywordResponse?> QueryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ICollection<KeywordResponse>> QueryByTypeAsync(
        KeywordType type,
        CancellationToken cancellationToken = default
    );
}
