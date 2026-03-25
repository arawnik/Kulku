using Kulku.Application.Projects.Models;
using Kulku.Domain;

namespace Kulku.Application.Projects.Ports;

/// <summary>
/// Read-side port for project queries.
/// </summary>
public interface IProjectQueries
{
    /// <summary>
    /// Returns all projects with a single translated set per the given language.
    /// Used by the public-facing API/UI.
    /// </summary>
    Task<IReadOnlyList<ProjectModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns all projects with every available translation and keyword IDs.
    /// Used by admin views that display and edit content across all languages.
    /// </summary>
    Task<IReadOnlyList<ProjectTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a single project with all translations and keyword IDs.
    /// Returns <c>null</c> when the project does not exist.
    /// </summary>
    Task<ProjectTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid projectId,
        CancellationToken cancellationToken = default
    );
}
