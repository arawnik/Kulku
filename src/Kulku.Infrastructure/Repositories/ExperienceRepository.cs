using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing experience entries and their full localization graph.
/// </summary>
public class ExperienceRepository(AppDbContext context) : IExperienceRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Experience experience)
    {
        _context.Experiences.Add(experience);
    }

    public void Remove(Experience experience)
    {
        _context.Remove(experience);
    }

    public async Task<Experience?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Experiences.Where(e => e.Id == id)
            .Include(e => e.Translations)
            .Include(e => e.Company)
            .ThenInclude(ec => ec.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
