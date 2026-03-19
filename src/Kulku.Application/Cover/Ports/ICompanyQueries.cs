using Kulku.Application.Cover.Models;

namespace Kulku.Application.Cover.Ports;

/// <summary>
/// Read-side port for company queries.
/// </summary>
public interface ICompanyQueries
{
    /// <summary>
    /// Returns all companies with every available translation and experience count.
    /// </summary>
    Task<IReadOnlyList<CompanyTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a single company with all translations for editing.
    /// </summary>
    Task<CompanyTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid companyId,
        CancellationToken cancellationToken = default
    );
}
