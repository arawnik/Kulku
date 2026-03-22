using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Ideas;

/// <summary>
/// EF Core implementation of idea domain read-side queries.
/// </summary>
public class IdeaDomainQueries(AppDbContext context) : IIdeaDomainQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<IdeaDomainModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .IdeaDomains.AsNoTracking()
            .OrderBy(d => d.Order)
            .LeftJoin(
                _context.IdeaDomainTranslations.Where(t => t.Language == language),
                d => d.Id,
                dt => dt.IdeaDomainId,
                (d, dt) => new { d, dt }
            )
            .Select(x => new IdeaDomainModel(
                x.d.Id,
                x.dt != null ? x.dt.Name : string.Empty,
                x.d.Icon,
                x.d.Order
            ))
            .ToListAsync(cancellationToken);

        return result;
    }
}
