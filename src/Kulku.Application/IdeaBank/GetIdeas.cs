using Kulku.Application.Abstractions.Localization;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Retrieves all ideas with lightweight data for the list view.
/// </summary>
public static class GetIdeas
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<IdeaListModel>>,
            ILocalizedRequest;

    internal sealed class Handler(IIdeaQueries queries)
        : IQueryHandler<Query, IReadOnlyList<IdeaListModel>>
    {
        private readonly IIdeaQueries _queries = queries;

        public async Task<Result<IReadOnlyList<IdeaListModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(query.Language, cancellationToken));
        }
    }
}
