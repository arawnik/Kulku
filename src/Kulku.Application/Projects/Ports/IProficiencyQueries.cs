using Kulku.Application.Projects.Models;

namespace Kulku.Application.Projects.Ports;

/// <summary>
/// Read-side port for proficiency level queries.
/// </summary>
public interface IProficiencyQueries
{
    /// <summary>
    /// Returns all proficiency levels with translations, ordered by Order.
    /// Includes a count of keywords referencing each level.
    /// </summary>
    Task<IReadOnlyList<ProficiencyTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds a single proficiency level by ID with all translations.
    /// </summary>
    Task<ProficiencyTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid proficiencyId,
        CancellationToken cancellationToken = default
    );
}
