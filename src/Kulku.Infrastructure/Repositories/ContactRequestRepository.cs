using Kulku.Domain.Contacts;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing contact request entries.
/// </summary>
public class ContactRequestRepository(AppDbContext context) : IContactRequestRepository
{
    private readonly AppDbContext _context = context;

    public void Add(ContactRequest request)
    {
        request.Timestamp = DateTime.UtcNow;
        _context.ContactRequests.Add(request);
    }

    public void Remove(ContactRequest request)
    {
        _context.Remove(request);
    }

    public async Task<ContactRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .ContactRequests.Where(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
