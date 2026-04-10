using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Company;

/// <summary>
/// Retrieves companies that are not yet enrolled in network tracking.
/// </summary>
public static class GetAvailableNetworkCompanies
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<NetworkAvailableCompanyModel>>,
            ILocalizedRequest;

    internal sealed class Handler(INetworkCompanyQueries queries)
        : IQueryHandler<Query, IReadOnlyList<NetworkAvailableCompanyModel>>
    {
        private readonly INetworkCompanyQueries _queries = queries;

        public async Task<Result<IReadOnlyList<NetworkAvailableCompanyModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.ListAvailableAsync(query.Language, cancellationToken)
            );
        }
    }
}
