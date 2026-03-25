using Kulku.Application.Abstractions.Localization;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Retrieves all idea statuses ordered by display order.
/// </summary>
public static class GetIdeaStatuses
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<IdeaStatusModel>>,
            ILocalizedRequest;

    internal sealed class Handler(IIdeaStatusQueries queries)
        : IQueryHandler<Query, IReadOnlyList<IdeaStatusModel>>
    {
        private readonly IIdeaStatusQueries _queries = queries;

        public async Task<Result<IReadOnlyList<IdeaStatusModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(query.Language, cancellationToken));
        }
    }
}
