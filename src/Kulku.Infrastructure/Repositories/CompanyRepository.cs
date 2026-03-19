using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing companies and their translations.
/// </summary>
public class CompanyRepository(AppDbContext context) : ICompanyRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Company company)
    {
        _context.Companies.Add(company);
    }

    public void Remove(Company company)
    {
        _context.Remove(company);
    }

    public async Task<Company?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Companies.Where(c => c.Id == id)
            .Include(c => c.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
