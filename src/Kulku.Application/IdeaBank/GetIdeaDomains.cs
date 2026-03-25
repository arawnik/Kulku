using Kulku.Application.Abstractions.Localization;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Retrieves all idea domains ordered by display order.
/// </summary>
public static class GetIdeaDomains
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<IdeaDomainModel>>,
            ILocalizedRequest;

    internal sealed class Handler(IIdeaDomainQueries queries)
        : IQueryHandler<Query, IReadOnlyList<IdeaDomainModel>>
    {
        private readonly IIdeaDomainQueries _queries = queries;

        public async Task<Result<IReadOnlyList<IdeaDomainModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(query.Language, cancellationToken));
        }
    }
}
