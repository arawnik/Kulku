using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing introduction and their full localization graph.
/// </summary>
public class IntroductionRepository(AppDbContext context) : IIntroductionRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Introduction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Introductions.Where(i => i.Id == id)
            .Include(i => i.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(Introduction introduction)
    {
        _context.Introductions.Add(introduction);
    }

    public void Remove(Introduction introduction)
    {
        _context.Introductions.Remove(introduction);
    }
}
