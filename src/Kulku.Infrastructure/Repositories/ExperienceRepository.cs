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
            .Select(e => new
            {
                e.Id,
                Translation = e
                    .Translations.Where(t => t.Language == language)
                    .Select(t => new { t.Title, t.Description })
                    .FirstOrDefault(),
                Company = new
                {
                    Translation = e
                        .Company.Translations.Where(t => t.Language == language)
                        .Select(ct => new { ct.Name, ct.Description })
                        .FirstOrDefault(),
                },
                Keywords = e.Keywords.Select(ek => new
                {
                    Translation = ek
                        .Translations.Where(t => t.Language == language)
                        .Select(t => new { t.Name })
                        .FirstOrDefault(),
                    ek.Type,
                    Proficiency = new
                    {
                        Translation = ek
                            .Proficiency.Translations.Where(t => t.Language == language)
                            .Select(t => new { t.Name, t.Description })
                            .FirstOrDefault(),
                        ek.Proficiency.Scale,
                        ek.Proficiency.Order,
                    },
                    ek.Order,
                    ek.Display,
                }),
                e.StartDate,
                e.EndDate,
            })
            .ToListAsync(cancellationToken);

        return
        [
            .. result.Select(e => new ExperienceResponse(
                Id: e.Id,
                Title: e.Translation?.Title ?? string.Empty,
                Description: e.Translation?.Description ?? string.Empty,
                Company: new CompanyResponse(
                    Name: e.Company.Translation?.Name ?? string.Empty,
                    Description: e.Company.Translation?.Description ?? string.Empty
                ),
                Keywords:
                [
                    .. e.Keywords.Select(ek => new KeywordResponse(
                        Name: ek.Translation?.Name ?? string.Empty,
                        Type: ek.Type,
                        Proficiency: new ProficiencyResponse(
                            Name: ek.Proficiency.Translation?.Name ?? string.Empty,
                            Description: ek.Proficiency.Translation?.Description ?? string.Empty,
                            Scale: ek.Proficiency.Scale,
                            Order: ek.Proficiency.Order
                        ),
                        Order: ek.Order,
                        Display: ek.Display
                    )),
                ],
                StartDate: e.StartDate,
                EndDate: e.EndDate
            )),
        ];
    }
}
