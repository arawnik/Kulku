using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using Kulku.Domain;
using Kulku.Domain.Projects;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

public class KeywordQueries(AppDbContext context) : IKeywordQueries
{
    private readonly AppDbContext _context = context;

    public async Task<KeywordModel?> FindByIdAsync(
        Guid id,
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var baseQuery = _context.Keywords.Where(k => k.Id == id && k.Display);

        var result = await BuildKeywordQuery(language, baseQuery)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<IReadOnlyList<KeywordModel>> ListByTypeAsync(
        KeywordType type,
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var baseQuery = _context
            .Keywords.Where(k => k.Type == type && k.Display)
            .OrderBy(k => k.Order);

        var result = await BuildKeywordQuery(language, baseQuery).ToListAsync(cancellationToken);

        return result;
    }

    private IQueryable<KeywordModel> BuildKeywordQuery(
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
                    new KeywordModel(
                        Name: x.kt != null ? x.kt.Name : string.Empty,
                        Type: x.k.Type,
                        Proficiency: new ProficiencyModel(
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
