using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Contact;

/// <summary>
/// Retrieves network contacts, optionally filtered by company.
/// </summary>
public static class GetNetworkContacts
{
    public sealed record Query(LanguageCode Language, Guid? CompanyId = null)
        : IQuery<IReadOnlyList<NetworkContactModel>>,
            ILocalizedRequest;

    internal sealed class Handler(INetworkContactQueries queries)
        : IQueryHandler<Query, IReadOnlyList<NetworkContactModel>>
    {
        private readonly INetworkContactQueries _queries = queries;

        public async Task<Result<IReadOnlyList<NetworkContactModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            var contacts = query.CompanyId.HasValue
                ? await _queries.ListByCompanyAsync(
                    query.CompanyId.Value,
                    query.Language,
                    cancellationToken
                )
                : await _queries.ListUnaffiliatedAsync(cancellationToken);

            return Result.Success(contacts);
        }
    }
}
