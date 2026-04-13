using Kulku.Application.Contacts.Ports;
using Kulku.Domain.Contacts;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Contacts;

/// <summary>
/// Returns the count of contact requests with a given status.
/// </summary>
public static class GetContactRequestCountByStatus
{
    public sealed record Query(ContactRequestStatus Status = ContactRequestStatus.New)
        : IQuery<int>;

    internal sealed class Handler(IContactRequestQueries queries) : IQueryHandler<Query, int>
    {
        private readonly IContactRequestQueries _queries = queries;

        public async Task<Result<int>> Handle(Query query, CancellationToken cancellationToken)
        {
            var count = await _queries.CountByStatusAsync(query.Status, cancellationToken);
            return Result.Success(count);
        }
    }
}
