using Kulku.Contract.Enums;
using Kulku.Contract.Projects;
using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using Kulku.Infrastructure.Helpers;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing keywords and their full localization graph.
/// </summary>
public class KeywordRepository(AppDbContext context) : IKeywordRepository
{
    private readonly AppDbContext _context = context;

    public void Add(Keyword keyword)
    {
        _context.Keywords.Add(keyword);
    }

    public void Remove(Keyword keyword)
    {
        _context.Remove(keyword);
    }

    public async Task<Keyword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Keywords.Where(k => k.Id == id)
            .Include(k => k.Translations)
            .Include(k => k.Proficiency)
            .ThenInclude(kp => kp.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<KeywordResponse?> QueryByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var language = CultureLanguageHelper.GetCurrentLanguage();

        var result = await _context
            .Keywords.Where(k => k.Id == id && k.Display)
            .Select(k => new
            {
                k.Type,
                k.Order,
                k.Display,
                Translation = k
                    .Translations.Where(t => t.Language == language)
                    .Select(t => new { t.Name })
                    .FirstOrDefault(),
                Proficiency = new
                {
                    k.Proficiency.Scale,
                    k.Proficiency.Order,
                    Translation = k
                        .Proficiency.Translations.Where(t => t.Language == language)
                        .Select(pt => new { pt.Name, pt.Description })
                        .FirstOrDefault(),
                },
            })
            .FirstOrDefaultAsync(cancellationToken);

        return result is null
            ? null
            : new KeywordResponse(
                Name: result.Translation?.Name ?? string.Empty,
                Type: result.Type,
                Proficiency: new ProficiencyResponse(
                    Name: result.Proficiency.Translation?.Name ?? string.Empty,
                    Description: result.Proficiency.Translation?.Description ?? string.Empty,
                    Scale: result.Proficiency.Scale,
                    Order: result.Proficiency.Order
                ),
                Order: result.Order,
                Display: result.Display
            );
    }

    public async Task<ICollection<KeywordResponse>> QueryByTypeAsync(
        KeywordType type,
        CancellationToken cancellationToken = default
    )
    {
        var language = CultureLanguageHelper.GetCurrentLanguage();

        var result = await _context
            .Keywords.Where(k => k.Type == type && k.Display)
            .Select(k => new
            {
                k.Type,
                k.Order,
                k.Display,
                Translation = k
                    .Translations.Where(t => t.Language == language)
                    .Select(t => new { t.Name })
                    .FirstOrDefault(),

                Proficiency = new
                {
                    k.Proficiency.Scale,
                    k.Proficiency.Order,
                    Translation = k
                        .Proficiency.Translations.Where(t => t.Language == language)
                        .Select(pt => new { pt.Name, pt.Description })
                        .FirstOrDefault(),
                },
            })
            .OrderBy(k => k.Order)
            .ToListAsync(cancellationToken);

        return
        [
            .. result.Select(k => new KeywordResponse(
                Name: k.Translation?.Name ?? string.Empty,
                Type: k.Type,
                Proficiency: new ProficiencyResponse(
                    Name: k.Proficiency.Translation?.Name ?? string.Empty,
                    Description: k.Proficiency.Translation?.Description ?? string.Empty,
                    Scale: k.Proficiency.Scale,
                    Order: k.Proficiency.Order
                ),
                Order: k.Order,
                Display: k.Display
            )),
        ];
    }
}
