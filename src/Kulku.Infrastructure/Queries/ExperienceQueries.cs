using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Experience.Ports;
using Kulku.Application.Cover.Models;
using Kulku.Application.Projects.Models;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

/// <summary>
/// EF Core implementation of <see cref="IExperienceQueries"/>.
/// </summary>
public class ExperienceQueries(AppDbContext context) : IExperienceQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<ExperienceModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Experiences.AsNoTracking()
            .OrderBy(e => e.EndDate.HasValue)
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
            .Select(x => new ExperienceModel(
                Id: x.e.Id,
                Title: x.et != null ? x.et.Title : string.Empty,
                Description: x.et != null ? x.et.Description : string.Empty,
                Company: new CompanyModel(
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
                    .Select(k => new KeywordModel(
                        Name: k.KeywordName ?? string.Empty,
                        Type: k.Keyword.Type,
                        Proficiency: new ProficiencyModel(
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

    /// <inheritdoc />
    public async Task<IReadOnlyList<ExperienceTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Experiences.AsNoTracking()
            .OrderBy(e => e.EndDate.HasValue)
            .ThenByDescending(e => e.EndDate)
            .Select(e => new ExperienceTranslationsModel(
                ExperienceId: e.Id,
                CompanyId: e.CompanyId,
                StartDate: e.StartDate,
                EndDate: e.EndDate,
                Translations: e.Translations.Select(t => new ExperienceTranslationItem(
                        t.Language,
                        t.Title,
                        t.Description
                    ))
                    .ToList(),
                CompanyTranslations: e.Company.Translations.Select(ct => new CompanyTranslationItem(
                        ct.Language,
                        ct.Name,
                        ct.Description
                    ))
                    .ToList(),
                KeywordNames: Array.Empty<string>()
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<ExperienceTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid experienceId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Experiences.AsNoTracking()
            .Where(e => e.Id == experienceId)
            .Select(e => new ExperienceTranslationsModel(
                ExperienceId: e.Id,
                CompanyId: e.CompanyId,
                StartDate: e.StartDate,
                EndDate: e.EndDate,
                Translations: e.Translations.Select(t => new ExperienceTranslationItem(
                        t.Language,
                        t.Title,
                        t.Description
                    ))
                    .ToList(),
                CompanyTranslations: e.Company.Translations.Select(ct => new CompanyTranslationItem(
                        ct.Language,
                        ct.Name,
                        ct.Description
                    ))
                    .ToList(),
                KeywordNames: Array.Empty<string>()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
