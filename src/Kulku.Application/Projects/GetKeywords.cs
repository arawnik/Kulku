using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using Kulku.Domain;
using Kulku.Domain.Projects;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

public static class GetKeywords
{
    public sealed record Query(KeywordType Type, LanguageCode Language)
        : IQuery<IReadOnlyList<KeywordModel>>,
            ILocalizedRequest;

    internal sealed class Handler(IKeywordQueries queries)
        : IQueryHandler<Query, IReadOnlyList<KeywordModel>>
    {
        private readonly IKeywordQueries _queries = queries;

        public async Task<Result<IReadOnlyList<KeywordModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.ListByTypeAsync(query.Type, query.Language, cancellationToken)
            );
        }
    }
}
