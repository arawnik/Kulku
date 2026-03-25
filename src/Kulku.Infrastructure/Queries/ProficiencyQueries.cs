using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

/// <summary>
/// EF Core implementation of proficiency read-side queries.
/// </summary>
public class ProficiencyQueries(AppDbContext context) : IProficiencyQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProficiencyTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Proficiencies.AsNoTracking()
            .OrderBy(p => p.Order)
            .Select(p => new ProficiencyTranslationsModel(
                ProficiencyId: p.Id,
                Scale: p.Scale,
                Order: p.Order,
                KeywordCount: p.Keywords.Count,
                Translations: p.Translations.Select(t => new ProficiencyTranslationItem(
                        t.Language,
                        t.Name,
                        t.Description
                    ))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<ProficiencyTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid proficiencyId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Proficiencies.AsNoTracking()
            .Where(p => p.Id == proficiencyId)
            .Select(p => new ProficiencyTranslationsModel(
                ProficiencyId: p.Id,
                Scale: p.Scale,
                Order: p.Order,
                KeywordCount: p.Keywords.Count,
                Translations: p.Translations.Select(t => new ProficiencyTranslationItem(
                        t.Language,
                        t.Name,
                        t.Description
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
