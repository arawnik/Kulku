using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Models;
using Kulku.Domain;

namespace Kulku.Application.Cover.Experience.Ports;

/// <summary>
/// Read-side port for experience queries.
/// </summary>
public interface IExperienceQueries
{
    /// <summary>
    /// Returns all experience entries with translated content for the given language,
    /// ordered by end date (ongoing first, then most recent).
    /// </summary>
    Task<IReadOnlyList<ExperienceModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns all experience entries with every available translation,
    /// ordered by end date (ongoing first, then most recent).
    /// Used by admin views that need to display all languages.
    /// </summary>
    Task<IReadOnlyList<ExperienceTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a single experience entry with all its translations,
    /// or <c>null</c> if not found.
    /// </summary>
    Task<ExperienceTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid experienceId,
        CancellationToken cancellationToken = default
    );
}
