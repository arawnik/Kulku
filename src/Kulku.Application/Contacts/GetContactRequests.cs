using Kulku.Application.Contacts.Models;
using Kulku.Application.Contacts.Ports;
using Kulku.Domain.Contacts;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Contacts;

/// <summary>
/// Retrieves contact requests, optionally filtered by status.
/// </summary>
public static class GetContactRequests
{
    public sealed record Query(ContactRequestStatus? StatusFilter = null)
        : IQuery<IReadOnlyList<ContactRequestModel>>;

    internal sealed class Handler(IContactRequestQueries queries)
        : IQueryHandler<Query, IReadOnlyList<ContactRequestModel>>
    {
        private readonly IContactRequestQueries _queries = queries;

        public async Task<Result<IReadOnlyList<ContactRequestModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            var requests = await _queries.ListAsync(query.StatusFilter, cancellationToken);
            return Result.Success(requests);
        }
    }
}
