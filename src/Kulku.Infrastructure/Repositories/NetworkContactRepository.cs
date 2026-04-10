using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing network contacts.
/// </summary>
public class NetworkContactRepository(AppDbContext context) : INetworkContactRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public void Add(NetworkContact contact)
    {
        _context.NetworkContacts.Add(contact);
    }

    /// <inheritdoc />
    public void Remove(NetworkContact contact)
    {
        _context.Remove(contact);
    }

    /// <inheritdoc />
    public async Task<NetworkContact?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.NetworkContacts.FirstOrDefaultAsync(
            c => c.Id == id,
            cancellationToken
        );
    }
}
