using Kulku.Contract.Cover;
using Kulku.Contract.Projects;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Infrastructure.Helpers;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing experience entries and their full localization graph.
/// </summary>
public class ExperienceRepository(AppDbContext context) : IExperienceRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Experience experience)
    {
        _context.Experiences.Add(experience);
    }

    public void Remove(Experience experience)
    {
        _context.Remove(experience);
    }

    public async Task<Experience?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Experiences.Where(e => e.Id == id)
            .Include(e => e.Translations)
            .Include(e => e.Company)
            .ThenInclude(ec => ec.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ICollection<ExperienceResponse>> QueryAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var language = CultureLanguageHelper.GetCurrentLanguage();

        var result = await _context
            .Experiences.OrderBy(e => e.EndDate.HasValue)
            .ThenByDescending(e => e.EndDate)
            .LeftJoin(
                _context.ExperienceTranslations.Where(t => t.Language == language),
                e => e.Id,
                t => t.ExperienceId,
                (e, et) => new { e, et }
            )
            .LeftJoin(
                _context.CompanyTranslations.Where(t => t.Language == language),
                x => x.e.CompanyId,
                ct => ct.CompanyId,
                (x, ct) =>
                    new
                    {
                        x.e,
                        x.et,
                        ct,
                    }
            )
            .Select(x => new ExperienceResponse(
                Id: x.e.Id,
                Title: x.et != null ? x.et.Title : string.Empty,
                Description: x.et != null ? x.et.Description : string.Empty,
                Company: new CompanyResponse(
                    Name: x.ct != null ? x.ct.Name : string.Empty,
                    Description: x.ct != null ? x.ct.Description : string.Empty
                ),
                Keywords: x.e.Keywords.OrderBy(ek => ek.Order)
                    // Avoid multiple selects by shaping first
                    .Select(ek => new
                    {
                        Keyword = ek,
                        KeywordName = ek
                            .Translations.Where(t => t.Language == language)
                            .Select(t => t.Name)
                            .FirstOrDefault(),
                        ProficiencyTranslation = ek
                            .Proficiency.Translations.Where(t => t.Language == language)
                            .Select(t => new { t.Name, t.Description })
                            .FirstOrDefault(),
                    })
                    // then map into DTO
                    .Select(k => new KeywordResponse(
                        Name: k.KeywordName ?? string.Empty,
                        Type: k.Keyword.Type,
                        Proficiency: new ProficiencyResponse(
                            Name: k.ProficiencyTranslation != null
                                ? k.ProficiencyTranslation.Name
                                : string.Empty,
                            Description: k.ProficiencyTranslation != null
                                ? k.ProficiencyTranslation.Description
                                : string.Empty,
                            Scale: k.Keyword.Proficiency.Scale,
                            Order: k.Keyword.Proficiency.Order
                        ),
                        Order: k.Keyword.Order,
                        Display: k.Keyword.Display
                    ))
                    .ToList(),
                StartDate: x.e.StartDate,
                EndDate: x.e.EndDate
            ))
            .ToListAsync(cancellationToken);

        return result;
    }
}
