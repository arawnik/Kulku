using Kulku.Application.Abstractions.Localization;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Retrieves a single idea with full detail including notes, tags, and keywords.
/// </summary>
public static class GetIdeaDetail
{
    public sealed record Query(Guid IdeaId, LanguageCode Language)
        : IQuery<IdeaDetailModel?>,
            ILocalizedRequest;

    internal sealed class Handler(IIdeaQueries queries) : IQueryHandler<Query, IdeaDetailModel?>
    {
        private readonly IIdeaQueries _queries = queries;

        public async Task<Result<IdeaDetailModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            var idea = await _queries.FindByIdAsync(
                query.IdeaId,
                query.Language,
                cancellationToken
            );
            return Result.Success(idea);
        }
    }
}
