using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for managing proficiency levels and their translations.
/// </summary>
public class ProficiencyRepository(AppDbContext context) : IProficiencyRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public void Add(Proficiency proficiency)
    {
        _context.Proficiencies.Add(proficiency);
    }

    /// <inheritdoc />
    public void Remove(Proficiency proficiency)
    {
        _context.Remove(proficiency);
    }

    /// <inheritdoc />
    public async Task<Proficiency?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Proficiencies.Where(p => p.Id == id)
            .Include(p => p.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
