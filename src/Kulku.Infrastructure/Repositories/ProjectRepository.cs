using Kulku.Contract.Projects;
using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using Kulku.Infrastructure.Helpers;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing projects and their full localization graph.
/// </summary>
public class ProjectRepository(AppDbContext context) : IProjectRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Projects.Where(p => p.Id == id)
            .Include(p => p.Translations)
            .Include(p => p.ProjectKeywords)
            .ThenInclude(pk => pk.Keyword)
            .ThenInclude(k => k.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(Project project)
    {
        _context.Projects.Add(project);
    }

    public void Remove(Project project)
    {
        _context.Projects.Remove(project);
    }

    public async Task<ICollection<ProjectResponse>> QueryAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var language = CultureLanguageHelper.GetCurrentLanguage();

        var result = await _context
            .Projects.OrderBy(p => p.Order)
            .LeftJoin(
                _context.ProjectTranslations.Where(t => t.Language == language),
                p => p.Id,
                pt => pt.ProjectId,
                (p, pt) => new { p, pt }
            )
            .Select(x => new ProjectResponse(
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
                    .ToList()
            ))
            .ToListAsync(cancellationToken);
        return result;
    }
}
