using Kulku.Application.Abstractions.Localization;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Retrieves all idea priorities ordered by display order.
/// </summary>
public static class GetIdeaPriorities
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<IdeaPriorityModel>>,
            ILocalizedRequest;

    internal sealed class Handler(IIdeaPriorityQueries queries)
        : IQueryHandler<Query, IReadOnlyList<IdeaPriorityModel>>
    {
        private readonly IIdeaPriorityQueries _queries = queries;

        public async Task<Result<IReadOnlyList<IdeaPriorityModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(query.Language, cancellationToken));
        }
    }
}
