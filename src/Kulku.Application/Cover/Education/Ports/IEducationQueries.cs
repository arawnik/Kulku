using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Cover.Models;
using Kulku.Domain;

namespace Kulku.Application.Cover.Education.Ports;

/// <summary>
/// Read-side port for education queries.
/// </summary>
public interface IEducationQueries
{
    /// <summary>
    /// Returns all education entries with translated content for the given language,
    /// ordered by end date (ongoing first, then most recent).
    /// </summary>
    Task<IReadOnlyList<EducationModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns all education entries with every available translation,
    /// ordered by end date (ongoing first, then most recent).
    /// Used by admin views that need to display all languages.
    /// </summary>
    Task<IReadOnlyList<EducationTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a single education entry with all its translations,
    /// or <c>null</c> if not found.
    /// </summary>
    Task<EducationTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid educationId,
        CancellationToken cancellationToken = default
    );
}
