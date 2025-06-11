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
            .Include(p => p.Keywords)
            .ThenInclude(pk => pk.Keyword)
            .ThenInclude(k => k.Translations)
            .Include(p => p.Keywords)
            .ThenInclude(pk => pk.Keyword)
            .ThenInclude(k => k.Proficiency)
            .ThenInclude(pr => pr.Translations)
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
            .Projects.Select(p => new
            {
                p.Url,
                p.ImageUrl,
                p.Order,

                Translation = p
                    .Translations.Where(t => t.Language == language)
                    .Select(t => new
                    {
                        t.Name,
                        t.Info,
                        t.Description,
                    })
                    .FirstOrDefault(),

                Keywords = p.Keywords.Select(pk => new
                {
                    pk.Keyword.Type,
                    pk.Keyword.Order,
                    pk.Keyword.Display,

                    Translation = pk
                        .Keyword.Translations.Where(t => t.Language == language)
                        .Select(t => new { t.Name })
                        .FirstOrDefault(),

                    Proficiency = new
                    {
                        pk.Keyword.Proficiency.Scale,
                        pk.Keyword.Proficiency.Order,

                        Translation = pk
                            .Keyword.Proficiency.Translations.Where(t => t.Language == language)
                            .Select(t => new { t.Name, t.Description })
                            .FirstOrDefault(),
                    },
                }),
            })
            .OrderBy(p => p.Order)
            .ToListAsync(cancellationToken);

        return
        [
            .. result.Select(p => new ProjectResponse(
                Name: p.Translation?.Name ?? string.Empty,
                Info: p.Translation?.Info ?? string.Empty,
                Description: p.Translation?.Description ?? string.Empty,
                Url: p.Url,
                Order: p.Order,
                ImageUrl: p.ImageUrl,
                Keywords:
                [
                    .. p.Keywords.Select(k => new KeywordResponse(
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
                ]
            )),
        ];
    }
}
