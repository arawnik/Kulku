using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing institutions and their translations.
/// </summary>
public class InstitutionRepository(AppDbContext context) : IInstitutionRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public void Add(Institution institution)
    {
        _context.Institutions.Add(institution);
    }

    /// <inheritdoc />
    public void Remove(Institution institution)
    {
        _context.Remove(institution);
    }

    /// <inheritdoc />
    public async Task<Institution?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Institutions.Where(i => i.Id == id)
            .Include(i => i.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
