using Kulku.Domain.Ideas;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing ideas with their full object graph.
/// </summary>
public class IdeaRepository(AppDbContext context) : IIdeaRepository
{
    private readonly AppDbContext _context = context;

    ///<inheritdoc/>
    public void Add(Idea idea)
    {
        _context.Ideas.Add(idea);
    }

    ///<inheritdoc/>
    public void Remove(Idea idea)
    {
        _context.Remove(idea);
    }

    ///<inheritdoc/>
    public async Task<Idea?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Ideas.Where(i => i.Id == id)
            .Include(i => i.Domain)
            .Include(i => i.Notes.OrderByDescending(n => n.CreatedAt))
            .Include(i => i.IdeaIdeaTags)
            .ThenInclude(it => it.IdeaTag)
            .Include(i => i.IdeaKeywords)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
