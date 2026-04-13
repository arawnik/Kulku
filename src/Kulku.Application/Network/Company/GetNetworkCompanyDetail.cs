using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Company;

/// <summary>
/// Retrieves detailed information about a single enrolled company.
/// </summary>
public static class GetNetworkCompanyDetail
{
    public sealed record Query(Guid CompanyId, LanguageCode Language)
        : IQuery<NetworkCompanyDetailModel?>,
            ILocalizedRequest;

    internal sealed class Handler(INetworkCompanyQueries queries)
        : IQueryHandler<Query, NetworkCompanyDetailModel?>
    {
        private readonly INetworkCompanyQueries _queries = queries;

        public async Task<Result<NetworkCompanyDetailModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            var detail = await _queries.FindByCompanyIdAsync(
                query.CompanyId,
                query.Language,
                cancellationToken
            );
            return Result.Success(detail);
        }
    }
}
