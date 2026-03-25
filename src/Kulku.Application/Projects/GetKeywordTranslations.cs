using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Lists all keywords with translations for admin CRUD.
/// </summary>
public static class GetKeywordTranslations
{
    public sealed record Query() : IQuery<IReadOnlyList<KeywordTranslationsModel>>;

    internal sealed class Handler(IKeywordQueries queries)
        : IQueryHandler<Query, IReadOnlyList<KeywordTranslationsModel>>
    {
        private readonly IKeywordQueries _queries = queries;

        public async Task<Result<IReadOnlyList<KeywordTranslationsModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllWithTranslationsAsync(cancellationToken));
        }
    }
}
