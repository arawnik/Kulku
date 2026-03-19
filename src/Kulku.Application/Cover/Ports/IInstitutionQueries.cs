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
    /// Returns all institutions with every available translation and education count.
    /// </summary>
    Task<IReadOnlyList<InstitutionTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a single institution with all translations for editing.
    /// </summary>
    Task<InstitutionTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid institutionId,
        CancellationToken cancellationToken = default
    );
}
