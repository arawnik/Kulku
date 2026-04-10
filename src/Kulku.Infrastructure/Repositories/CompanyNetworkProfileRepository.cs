using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing company network profiles.
/// </summary>
public class CompanyNetworkProfileRepository(AppDbContext context)
    : ICompanyNetworkProfileRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public void Add(CompanyNetworkProfile profile)
    {
        _context.CompanyNetworkProfiles.Add(profile);
    }

    /// <inheritdoc />
    public void Remove(CompanyNetworkProfile profile)
    {
        _context.Remove(profile);
    }

    /// <summary>
    /// Finds a profile by its <b>CompanyId</b> (not the profile's own Id),
    /// including the category join entries.
    /// </summary>
    public async Task<CompanyNetworkProfile?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .CompanyNetworkProfiles.Where(p => p.CompanyId == id)
            .Include(p => p.CompanyNetworkProfileCategories)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
