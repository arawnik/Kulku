using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

/// <summary>
/// EF Core implementation of institution read-side queries.
/// </summary>
public class InstitutionQueries(AppDbContext context) : IInstitutionQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<InstitutionModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .InstitutionTranslations.AsNoTracking()
            .Where(t => t.Language == language)
            .Select(t => new InstitutionModel(t.Name, t.Department, t.Description))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<InstitutionTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Institutions.AsNoTracking()
            .Select(i => new InstitutionTranslationsModel(
                InstitutionId: i.Id,
                EducationCount: _context.Educations.Count(e => e.InstitutionId == i.Id),
                Translations: i.Translations.Select(t => new InstitutionTranslationItem(
                        t.Language,
                        t.Name,
                        t.Department,
                        t.Description
                    ))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<InstitutionTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid institutionId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Institutions.AsNoTracking()
            .Where(i => i.Id == institutionId)
            .Select(i => new InstitutionTranslationsModel(
                InstitutionId: i.Id,
                EducationCount: _context.Educations.Count(e => e.InstitutionId == i.Id),
                Translations: i.Translations.Select(t => new InstitutionTranslationItem(
                        t.Language,
                        t.Name,
                        t.Department,
                        t.Description
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
