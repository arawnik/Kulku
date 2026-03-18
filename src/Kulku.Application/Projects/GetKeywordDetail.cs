using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Gets a single keyword with translations for editing.
/// </summary>
public static class GetKeywordDetail
{
    public sealed record Query(Guid KeywordId) : IQuery<KeywordTranslationsModel?>;

    internal sealed class Handler(IKeywordQueries queries)
        : IQueryHandler<Query, KeywordTranslationsModel?>
    {
        private readonly IKeywordQueries _queries = queries;

        public async Task<Result<KeywordTranslationsModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.FindByIdWithTranslationsAsync(query.KeywordId, cancellationToken)
            );
        }
    }
}
