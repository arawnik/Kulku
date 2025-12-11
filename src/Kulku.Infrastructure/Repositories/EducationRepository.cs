using Kulku.Contract.Cover;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Infrastructure.Helpers;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing experience entries and their full localization graph.
/// </summary>
public class EducationRepository(AppDbContext context) : IEducationRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Education education)
    {
        _context.Educations.Add(education);
    }

    public void Remove(Education education)
    {
        _context.Remove(education);
    }

    public async Task<Education?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Educations.Where(p => p.Id == id)
            .Include(e => e.Translations)
            .Include(e => e.Institution)
            .ThenInclude(ei => ei.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<EducationResponse>> QueryAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var language = CultureLanguageHelper.GetCurrentLanguage();

        var result = await _context
            .Educations.OrderBy(e => e.EndDate.HasValue)
            .ThenByDescending(e => e.EndDate)
            .LeftJoin(
                _context.EducationTranslations.Where(t => t.Language == language),
                e => e.Id,
                t => t.EducationId,
                (e, t) => new { e, t }
            )
            .LeftJoin(
                _context.InstitutionTranslations.Where(it => it.Language == language),
                et => et.e.InstitutionId,
                it => it.InstitutionId,
                (et, it) =>
                    new EducationResponse(
                        Id: et.e.Id,
                        Title: et.t != null ? et.t.Title : string.Empty,
                        Description: et.t != null ? et.t.Description : string.Empty,
                        Institution: new InstitutionResponse(
                            Name: it != null ? it.Name : string.Empty,
                            Department: it != null ? it.Department : string.Empty,
                            Description: it != null ? it.Description : string.Empty
                        ),
                        StartDate: et.e.StartDate,
                        EndDate: et.e.EndDate
                    )
            )
            .ToListAsync(cancellationToken);
        return result;
    }
}
