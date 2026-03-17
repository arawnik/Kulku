using Kulku.Application.Abstractions.Rendering;
using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

public class ProjectQueries(AppDbContext context, IMarkdownRenderer markdownRenderer)
    : IProjectQueries
{
    private readonly AppDbContext _context = context;
    private readonly IMarkdownRenderer _markdownRenderer = markdownRenderer;

    public async Task<IReadOnlyList<ProjectModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Projects.OrderBy(p => p.Order)
            .LeftJoin(
                _context.ProjectTranslations.Where(t => t.Language == language),
                p => p.Id,
                pt => pt.ProjectId,
                (p, pt) => new { p, pt }
            )
            .Select(x => new ProjectModel(
                Name: x.pt != null ? x.pt.Name : string.Empty,
                Info: x.pt != null ? x.pt.Info : string.Empty,
                Description: x.pt != null ? x.pt.Description : string.Empty,
                Url: x.p.Url,
                Order: x.p.Order,
                ImageUrl: x.p.ImageUrl,
                Keywords: x.p.ProjectKeywords.OrderBy(pk => pk.Keyword.Order)
                    // Avoid multiple selects by shaping first
                    .Select(pk => new
                    {
                        pk.Keyword,
                        KeywordName = pk
                            .Keyword.Translations.Where(t => t.Language == language)
                            .Select(t => t.Name)
                            .FirstOrDefault(),
                        ProficiencyTranslation = pk
                            .Keyword.Proficiency.Translations.Where(t => t.Language == language)
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
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        // Convert markdown descriptions to rendered HTML for the public API.
        return
        [
            .. result.Select(p => p with { Description = _markdownRenderer.ToHtml(p.Description) }),
        ];
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProjectTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Projects.AsNoTracking()
            .OrderBy(p => p.Order)
            .Select(p => new ProjectTranslationsModel(
                ProjectId: p.Id,
                Url: p.Url,
                ImageUrl: p.ImageUrl,
                Order: p.Order,
                Translations: p.Translations.Select(t => new ProjectTranslationItem(
                        t.Language,
                        t.Name,
                        t.Info,
                        t.Description ?? string.Empty
                    ))
                    .ToList(),
                KeywordIds: p.ProjectKeywords.Select(pk => pk.KeywordId).ToList()
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<ProjectTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid projectId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Projects.AsNoTracking()
            .Where(p => p.Id == projectId)
            .Select(p => new ProjectTranslationsModel(
                ProjectId: p.Id,
                Url: p.Url,
                ImageUrl: p.ImageUrl,
                Order: p.Order,
                Translations: p.Translations.Select(t => new ProjectTranslationItem(
                        t.Language,
                        t.Name,
                        t.Info,
                        t.Description ?? string.Empty
                    ))
                    .ToList(),
                KeywordIds: p.ProjectKeywords.Select(pk => pk.KeywordId).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
