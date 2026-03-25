using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Ideas;

/// <summary>
/// EF Core implementation of idea tag read-side queries.
/// </summary>
public class IdeaTagQueries(AppDbContext context) : IIdeaTagQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<IdeaTagModel>> ListAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .IdeaTags.AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new IdeaTagModel(t.Id, t.Name, t.ColorHex, t.IdeaIdeaTags.Count))
            .ToListAsync(cancellationToken);
    }
}
