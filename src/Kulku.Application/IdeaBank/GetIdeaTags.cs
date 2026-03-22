using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Retrieves all idea tags with their reference counts.
/// </summary>
public static class GetIdeaTags
{
    public sealed record Query() : IQuery<IReadOnlyList<IdeaTagModel>>;

    internal sealed class Handler(IIdeaTagQueries queries)
        : IQueryHandler<Query, IReadOnlyList<IdeaTagModel>>
    {
        private readonly IIdeaTagQueries _queries = queries;

        public async Task<Result<IReadOnlyList<IdeaTagModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(cancellationToken));
        }
    }
}
