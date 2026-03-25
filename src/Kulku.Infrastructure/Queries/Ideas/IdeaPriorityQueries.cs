using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Ideas;

/// <summary>
/// EF Core implementation of idea priority read-side queries.
/// </summary>
public class IdeaPriorityQueries(AppDbContext context) : IIdeaPriorityQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<IdeaPriorityModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .IdeaPriorities.AsNoTracking()
            .OrderBy(p => p.Order)
            .LeftJoin(
                _context.IdeaPriorityTranslations.Where(t => t.Language == language),
                p => p.Id,
                pt => pt.IdeaPriorityId,
                (p, pt) => new { p, pt }
            )
            .Select(x => new IdeaPriorityModel(
                x.p.Id,
                x.pt != null ? x.pt.Name : string.Empty,
                x.p.Style,
                x.p.Order
            ))
            .ToListAsync(cancellationToken);

        return result;
    }
}
