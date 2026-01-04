using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing keywords and their full localization graph.
/// </summary>
public class KeywordRepository(AppDbContext context) : IKeywordRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Keyword keyword)
    {
        _context.Keywords.Add(keyword);
    }

    public void Remove(Keyword keyword)
    {
        _context.Remove(keyword);
    }

    public async Task<Keyword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Keywords.Where(k => k.Id == id)
            .Include(k => k.Translations)
            .Include(k => k.Proficiency)
            .ThenInclude(kp => kp.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
