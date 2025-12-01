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

        var baseQuery = _context.Keywords.Where(k => k.Id == id && k.Display);

        var result = await BuildKeywordQuery(language, baseQuery)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<ICollection<KeywordResponse>> QueryByTypeAsync(
        KeywordType type,
        CancellationToken cancellationToken = default
    )
    {
        var language = CultureLanguageHelper.GetCurrentLanguage();

        var baseQuery = _context
            .Keywords.Where(k => k.Type == type && k.Display)
            .OrderBy(k => k.Order);

        var result = await BuildKeywordQuery(language, baseQuery).ToListAsync(cancellationToken);

        return result;
    }

    private IQueryable<KeywordResponse> BuildKeywordQuery(
        LanguageCode language,
        IQueryable<Keyword> baseQuery
    )
    {
        return baseQuery
            .LeftJoin(
                _context.KeywordTranslations.Where(t => t.Language == language),
                k => k.Id,
                t => t.KeywordId,
                (k, kt) => new { k, kt }
            )
            .LeftJoin(
                _context.ProficiencyTranslations.Where(pt => pt.Language == language),
                x => x.k.ProficiencyId,
                pt => pt.ProficiencyId,
                (x, pt) =>
                    new KeywordResponse(
                        Name: x.kt != null ? x.kt.Name : string.Empty,
                        Type: x.k.Type,
                        Proficiency: new ProficiencyResponse(
                            Name: pt != null ? pt.Name : string.Empty,
                            Description: pt != null ? pt.Description : string.Empty,
                            Scale: x.k.Proficiency.Scale,
                            Order: x.k.Proficiency.Order
                        ),
                        Order: x.k.Order,
                        Display: x.k.Display
                    )
            );
    }
}
