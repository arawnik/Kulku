using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing experience entries and their full localization graph.
/// </summary>
public class EducationRepository(AppDbContext context) : IEducationRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Education education)
    {
        _context.Educations.Add(education);
    }

    public void Remove(Education education)
    {
        _context.Remove(education);
    }

    public async Task<Education?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Educations.Where(p => p.Id == id)
            .Include(e => e.Translations)
            .Include(e => e.Institution)
            .ThenInclude(ei => ei.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
