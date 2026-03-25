using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Ideas;

/// <summary>
/// EF Core implementation of idea status read-side queries.
/// </summary>
public class IdeaStatusQueries(AppDbContext context) : IIdeaStatusQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<IdeaStatusModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .IdeaStatuses.AsNoTracking()
            .OrderBy(s => s.Order)
            .LeftJoin(
                _context.IdeaStatusTranslations.Where(t => t.Language == language),
                s => s.Id,
                st => st.IdeaStatusId,
                (s, st) => new { s, st }
            )
            .Select(x => new IdeaStatusModel(
                x.s.Id,
                x.st != null ? x.st.Name : string.Empty,
                x.s.Style,
                x.s.Order
            ))
            .ToListAsync(cancellationToken);

        return result;
    }
}
