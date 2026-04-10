using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing network categories.
/// </summary>
public class NetworkCategoryRepository(AppDbContext context) : INetworkCategoryRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public void Add(NetworkCategory category)
    {
        _context.NetworkCategories.Add(category);
    }

    /// <inheritdoc />
    public void Remove(NetworkCategory category)
    {
        _context.Remove(category);
    }

    /// <inheritdoc />
    public async Task<NetworkCategory?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.NetworkCategories.FirstOrDefaultAsync(
            c => c.Id == id,
            cancellationToken
        );
    }
}
