using Kulku.Application.Cover.Models;
using Kulku.Domain;

namespace Kulku.Application.Cover.Ports;

/// <summary>
/// Read-side port for institution queries.
/// </summary>
public interface IInstitutionQueries
{
    /// <summary>
    /// Returns all institutions with a single translated name per the given language.
    /// </summary>
    Task<IReadOnlyList<InstitutionModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns all institutions with every available translation.
    /// Useful for admin views that need to show/edit all languages.
    /// </summary>
    Task<IReadOnlyList<InstitutionTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );
}
