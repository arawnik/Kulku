using Kulku.Contract.Enums;
using Kulku.Contract.Projects;
using Kulku.Domain.Projects;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Domain.Repositories;

public interface IKeywordRepository : IRepository
{
    Task<Keyword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Keyword keyword);
    void Remove(Keyword keyword);

    Task<KeywordResponse?> QueryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ICollection<KeywordResponse>> QueryByTypeAsync(
        KeywordType type,
        CancellationToken cancellationToken = default
    );
}
