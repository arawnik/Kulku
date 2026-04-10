using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Network;

/// <summary>
/// EF Core implementation of <see cref="INetworkCategoryQueries"/>.
/// </summary>
public class NetworkCategoryQueries(AppDbContext context) : INetworkCategoryQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<NetworkCategoryModel>> ListAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .NetworkCategories.AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new NetworkCategoryModel(c.Id, c.Name, c.ColorToken))
            .ToListAsync(cancellationToken);
    }
}
