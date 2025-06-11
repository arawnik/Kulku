using Kulku.Contract.Enums;
using Kulku.Contract.Projects;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

public static class GetKeywords
{
    public sealed record Query(KeywordType Type) : IQuery<ICollection<KeywordResponse>>;

    internal sealed class Handler(IKeywordRepository repository)
        : IQueryHandler<Query, ICollection<KeywordResponse>>
    {
        private readonly IKeywordRepository _repository = repository;

        public async Task<Result<ICollection<KeywordResponse>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _repository.QueryByTypeAsync(query.Type, cancellationToken)
            );
        }
    }
}
