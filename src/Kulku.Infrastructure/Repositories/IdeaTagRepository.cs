using Kulku.Domain.Ideas;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing idea tags.
/// </summary>
public class IdeaTagRepository(AppDbContext context) : IIdeaTagRepository
{
    private readonly AppDbContext _context = context;

    ///<inheritdoc/>
    public void Add(IdeaTag tag)
    {
        _context.IdeaTags.Add(tag);
    }

    ///<inheritdoc/>
    public void Remove(IdeaTag tag)
    {
        _context.Remove(tag);
    }

    ///<inheritdoc/>
    public async Task<IdeaTag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .IdeaTags.Where(t => t.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
