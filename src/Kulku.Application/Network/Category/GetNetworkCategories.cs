using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Category;

/// <summary>
/// Retrieves all network categories.
/// </summary>
public static class GetNetworkCategories
{
    public sealed record Query() : IQuery<IReadOnlyList<NetworkCategoryModel>>;

    internal sealed class Handler(INetworkCategoryQueries queries)
        : IQueryHandler<Query, IReadOnlyList<NetworkCategoryModel>>
    {
        private readonly INetworkCategoryQueries _queries = queries;

        public async Task<Result<IReadOnlyList<NetworkCategoryModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(cancellationToken));
        }
    }
}
