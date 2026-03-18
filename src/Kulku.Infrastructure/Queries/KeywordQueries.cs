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

    public async Task<IReadOnlyList<KeywordPickerModel>> ListAllForPickerAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Keywords.AsNoTracking()
            .OrderBy(k => k.Type)
            .ThenBy(k => k.Order)
            .Select(k => new KeywordPickerModel(
                k.Id,
                k.Translations.Select(t => t.Name).FirstOrDefault() ?? string.Empty,
                k.Type
            ))
            .ToListAsync(cancellationToken);

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

    /// <inheritdoc />
    public async Task<IReadOnlyList<KeywordTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Keywords.AsNoTracking()
            .OrderBy(k => k.Type)
            .ThenBy(k => k.Order)
            .Select(k => new KeywordTranslationsModel(
                KeywordId: k.Id,
                Type: k.Type,
                Order: k.Order,
                Display: k.Display,
                ProficiencyId: k.ProficiencyId,
                ProficiencyName: k.Proficiency.Translations.Select(t => t.Name).FirstOrDefault()
                    ?? string.Empty,
                ProficiencyScale: k.Proficiency.Scale,
                Translations: k.Translations.Select(t => new KeywordTranslationItem(
                        t.Language,
                        t.Name
                    ))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<KeywordTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid keywordId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Keywords.AsNoTracking()
            .Where(k => k.Id == keywordId)
            .Select(k => new KeywordTranslationsModel(
                KeywordId: k.Id,
                Type: k.Type,
                Order: k.Order,
                Display: k.Display,
                ProficiencyId: k.ProficiencyId,
                ProficiencyName: k.Proficiency.Translations.Select(t => t.Name).FirstOrDefault()
                    ?? string.Empty,
                ProficiencyScale: k.Proficiency.Scale,
                Translations: k.Translations.Select(t => new KeywordTranslationItem(
                        t.Language,
                        t.Name
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
