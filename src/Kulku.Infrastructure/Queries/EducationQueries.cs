using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

public class EducationQueries(AppDbContext context) : IEducationQueries
{
    private readonly AppDbContext _context = context;

    public async Task<IReadOnlyList<EducationModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
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
                    new EducationModel(
                        Id: et.e.Id,
                        Title: et.t != null ? et.t.Title : string.Empty,
                        Description: et.t != null ? et.t.Description : string.Empty,
                        Institution: new InstitutionModel(
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
