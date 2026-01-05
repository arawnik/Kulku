using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

public class ProjectQueries(AppDbContext context) : IProjectQueries
{
    private readonly AppDbContext _context = context;

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
        return result;
    }
}
