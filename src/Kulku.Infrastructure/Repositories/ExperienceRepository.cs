using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing experience entries and their full localization graph.
/// </summary>
public class ExperienceRepository(AppDbContext context) : IExperienceRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public void Add(Experience experience)
    {
        _context.Experiences.Add(experience);
    }

    /// <inheritdoc />
    public void Remove(Experience experience)
    {
        _context.Remove(experience);
    }

    /// <inheritdoc />
    public async Task<Experience?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Experiences.Where(e => e.Id == id)
            .Include(e => e.Translations)
            .Include(e => e.Keywords)
            .Include(e => e.Company)
            .ThenInclude(ec => ec.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SyncKeywordsAsync(
        Experience experience,
        IReadOnlyList<Guid> keywordIds,
        CancellationToken cancellationToken = default
    )
    {
        experience.Keywords.Clear();

        if (keywordIds.Count > 0)
        {
            var keywords = await _context
                .Keywords.Where(k => keywordIds.Contains(k.Id))
                .ToListAsync(cancellationToken);

            foreach (var kw in keywords)
                experience.Keywords.Add(kw);
        }
    }
}
