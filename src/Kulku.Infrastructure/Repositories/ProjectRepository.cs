using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
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
}
