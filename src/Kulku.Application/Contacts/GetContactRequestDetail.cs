using Kulku.Application.Contacts.Models;
using Kulku.Application.Contacts.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Contacts;

/// <summary>
/// Retrieves a single contact request by its identifier.
/// </summary>
public static class GetContactRequestDetail
{
    public sealed record Query(Guid Id) : IQuery<ContactRequestModel?>;

    internal sealed class Handler(IContactRequestQueries queries)
        : IQueryHandler<Query, ContactRequestModel?>
    {
        private readonly IContactRequestQueries _queries = queries;

        public async Task<Result<ContactRequestModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            var request = await _queries.FindByIdAsync(query.Id, cancellationToken);
            return Result.Success(request);
        }
    }
}
