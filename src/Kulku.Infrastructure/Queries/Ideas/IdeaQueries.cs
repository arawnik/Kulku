using Kulku.Application.IdeaBank.Models;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Application.Projects.Models;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Ideas;

/// <summary>
/// EF Core implementation of idea read-side queries.
/// </summary>
public class IdeaQueries(AppDbContext context) : IIdeaQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<IdeaListModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Ideas.AsNoTracking()
            .OrderByDescending(i => i.UpdatedAt)
            .LeftJoin(
                _context.IdeaStatusTranslations.Where(t => t.Language == language),
                i => i.StatusId,
                st => st.IdeaStatusId,
                (i, st) => new { i, st }
            )
            .LeftJoin(
                _context.IdeaPriorityTranslations.Where(t => t.Language == language),
                x => x.i.PriorityId,
                pt => pt.IdeaPriorityId,
                (x, pt) =>
                    new
                    {
                        x.i,
                        x.st,
                        pt,
                    }
            )
            .LeftJoin(
                _context.IdeaDomainTranslations.Where(t => t.Language == language),
                x => x.i.DomainId,
                dt => dt.IdeaDomainId,
                (x, dt) =>
                    new
                    {
                        x.i,
                        x.st,
                        x.pt,
                        dt,
                    }
            )
            .Select(x => new IdeaListModel(
                x.i.Id,
                x.i.Title,
                x.i.Summary,
                x.i.StatusId,
                x.st != null ? x.st.Name : string.Empty,
                x.i.Status.Style,
                x.i.PriorityId,
                x.pt != null ? x.pt.Name : string.Empty,
                x.i.Priority.Style,
                x.i.DomainId,
                x.dt != null ? x.dt.Name : string.Empty,
                x.i.Domain.Icon,
                x.i.IdeaIdeaTags.Select(it => it.IdeaTag.Name).ToList(),
                x.i.IdeaKeywords.Select(ik =>
                        ik.Keyword.Translations.Where(t => t.Language == language)
                            .Select(t => t.Name)
                            .FirstOrDefault() ?? string.Empty
                    )
                    .ToList(),
                x.i.Notes.Count,
                x.i.CreatedAt,
                x.i.UpdatedAt
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<IdeaDetailModel?> FindByIdAsync(
        Guid id,
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Ideas.AsNoTracking()
            .Where(i => i.Id == id)
            .LeftJoin(
                _context.IdeaStatusTranslations.Where(t => t.Language == language),
                i => i.StatusId,
                st => st.IdeaStatusId,
                (i, st) => new { i, st }
            )
            .LeftJoin(
                _context.IdeaPriorityTranslations.Where(t => t.Language == language),
                x => x.i.PriorityId,
                pt => pt.IdeaPriorityId,
                (x, pt) =>
                    new
                    {
                        x.i,
                        x.st,
                        pt,
                    }
            )
            .LeftJoin(
                _context.IdeaDomainTranslations.Where(t => t.Language == language),
                x => x.i.DomainId,
                dt => dt.IdeaDomainId,
                (x, dt) =>
                    new
                    {
                        x.i,
                        x.st,
                        x.pt,
                        dt,
                    }
            )
            .Select(x => new IdeaDetailModel(
                x.i.Id,
                x.i.Title,
                x.i.Summary,
                x.i.Description,
                x.i.StatusId,
                x.st != null ? x.st.Name : string.Empty,
                x.i.Status.Style,
                x.i.PriorityId,
                x.pt != null ? x.pt.Name : string.Empty,
                x.i.Priority.Style,
                x.i.DomainId,
                x.dt != null ? x.dt.Name : string.Empty,
                x.i.Domain.Icon,
                x.i.IdeaIdeaTags.Select(it => new IdeaTagModel(
                        it.IdeaTag.Id,
                        it.IdeaTag.Name,
                        it.IdeaTag.ColorHex,
                        it.IdeaTag.IdeaIdeaTags.Count
                    ))
                    .ToList(),
                x.i.IdeaKeywords.Select(ik => new KeywordPickerModel(
                        ik.Keyword.Id,
                        ik.Keyword.Translations.Where(t => t.Language == language)
                            .Select(t => t.Name)
                            .FirstOrDefault() ?? string.Empty,
                        ik.Keyword.Type
                    ))
                    .ToList(),
                x.i.Notes.OrderByDescending(n => n.CreatedAt)
                    .Select(n => new IdeaNoteModel(n.Id, n.Content, n.CreatedAt))
                    .ToList(),
                x.i.CreatedAt,
                x.i.UpdatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
