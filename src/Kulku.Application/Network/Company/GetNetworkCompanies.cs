using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Company;

/// <summary>
/// Retrieves all companies enrolled in the professional network.
/// </summary>
public static class GetNetworkCompanies
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<NetworkCompanyModel>>,
            ILocalizedRequest;

    internal sealed class Handler(INetworkCompanyQueries queries)
        : IQueryHandler<Query, IReadOnlyList<NetworkCompanyModel>>
    {
        private readonly INetworkCompanyQueries _queries = queries;

        public async Task<Result<IReadOnlyList<NetworkCompanyModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.ListEnrolledAsync(query.Language, cancellationToken)
            );
        }
    }
}
