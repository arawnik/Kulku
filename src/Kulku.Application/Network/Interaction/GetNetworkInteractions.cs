using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Interaction;

/// <summary>
/// Retrieves network interactions, optionally filtered by company.
/// </summary>
public static class GetNetworkInteractions
{
    public sealed record Query(LanguageCode Language, Guid? CompanyId = null)
        : IQuery<IReadOnlyList<NetworkInteractionModel>>,
            ILocalizedRequest;

    internal sealed class Handler(INetworkInteractionQueries queries)
        : IQueryHandler<Query, IReadOnlyList<NetworkInteractionModel>>
    {
        private readonly INetworkInteractionQueries _queries = queries;

        public async Task<Result<IReadOnlyList<NetworkInteractionModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            var interactions = query.CompanyId.HasValue
                ? await _queries.ListByCompanyAsync(
                    query.CompanyId.Value,
                    query.Language,
                    cancellationToken
                )
                : await _queries.ListAllAsync(query.Language, cancellationToken);

            return Result.Success(interactions);
        }
    }
}
