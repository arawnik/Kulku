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
            .Select(e => new
            {
                e.Id,
                Translation = e
                    .Translations.Where(t => t.Language == language)
                    .Select(t => new { t.Title, t.Description })
                    .FirstOrDefault(),
                Institution = new
                {
                    Translation = e
                        .Institution.Translations.Where(t => t.Language == language)
                        .Select(et => new
                        {
                            et.Name,
                            et.Department,
                            et.Description,
                        })
                        .FirstOrDefault(),
                },
                e.StartDate,
                e.EndDate,
            })
            .ToListAsync(cancellationToken);

        return
        [
            .. result.Select(e => new EducationResponse(
                Id: e.Id,
                Title: e.Translation?.Title ?? string.Empty,
                Description: e.Translation?.Description ?? string.Empty,
                Institution: new InstitutionResponse(
                    Name: e.Institution.Translation?.Name ?? string.Empty,
                    Department: e.Institution.Translation?.Department ?? string.Empty,
                    Description: e.Institution.Translation?.Description ?? string.Empty
                ),
                StartDate: e.StartDate,
                EndDate: e.EndDate
            )),
        ];
    }
}
