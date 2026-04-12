using Kulku.Application.Contacts.Models;
using Kulku.Application.Contacts.Ports;
using Kulku.Domain.Contacts;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

/// <summary>
/// EF Core implementation of <see cref="IContactRequestQueries"/>.
/// </summary>
public class ContactRequestQueries(AppDbContext context) : IContactRequestQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<ContactRequestModel>> ListAsync(
        ContactRequestStatus? statusFilter = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.ContactRequests.AsNoTracking().AsQueryable();

        if (statusFilter.HasValue)
            query = query.Where(r => r.Status == statusFilter.Value);

        return await query
            .OrderByDescending(r => r.Timestamp)
            .Select(r => new ContactRequestModel(
                r.Id,
                r.Name,
                r.Email,
                r.Subject,
                r.Message,
                r.Timestamp,
                r.Status
            ))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ContactRequestModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ContactRequests.AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new ContactRequestModel(
                r.Id,
                r.Name,
                r.Email,
                r.Subject,
                r.Message,
                r.Timestamp,
                r.Status
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountByStatusAsync(
        ContactRequestStatus status,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ContactRequests.AsNoTracking()
            .CountAsync(r => r.Status == status, cancellationToken);
    }
}
