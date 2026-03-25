using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing keywords and their full localization graph.
/// </summary>
public class KeywordRepository(AppDbContext context) : IKeywordRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public void Add(Keyword keyword)
    {
        _context.Keywords.Add(keyword);
    }

    /// <inheritdoc />
    public void Remove(Keyword keyword)
    {
        _context.Remove(keyword);
    }

    /// <inheritdoc />
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
