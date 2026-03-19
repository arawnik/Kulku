using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

public class CompanyQueries(AppDbContext context) : ICompanyQueries
{
    private readonly AppDbContext _context = context;

    public async Task<IReadOnlyList<CompanyTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Companies.AsNoTracking()
            .Select(c => new CompanyTranslationsModel(
                CompanyId: c.Id,
                ExperienceCount: _context.Experiences.Count(e => e.CompanyId == c.Id),
                Translations: c.Translations.Select(t => new CompanyTranslationItem(
                        t.Language,
                        t.Name,
                        t.Description
                    ))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<CompanyTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid companyId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Companies.AsNoTracking()
            .Where(c => c.Id == companyId)
            .Select(c => new CompanyTranslationsModel(
                CompanyId: c.Id,
                ExperienceCount: _context.Experiences.Count(e => e.CompanyId == c.Id),
                Translations: c.Translations.Select(t => new CompanyTranslationItem(
                        t.Language,
                        t.Name,
                        t.Description
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
