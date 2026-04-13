using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing network interactions.
/// </summary>
public class NetworkInteractionRepository(AppDbContext context) : INetworkInteractionRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public void Add(NetworkInteraction interaction)
    {
        _context.NetworkInteractions.Add(interaction);
    }

    /// <inheritdoc />
    public void Remove(NetworkInteraction interaction)
    {
        _context.Remove(interaction);
    }

    /// <inheritdoc />
    public async Task<NetworkInteraction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.NetworkInteractions.FirstOrDefaultAsync(
            i => i.Id == id,
            cancellationToken
        );
    }
}
