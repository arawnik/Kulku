using Kulku.Application.Cover.Models;

namespace Kulku.Application.Cover.Ports;

/// <summary>
/// Read-side port for company queries.
/// </summary>
public interface ICompanyQueries
{
    /// <summary>
    /// Returns all companies with every available translation.
    /// Useful for admin views that need to show/edit all languages.
    /// </summary>
    Task<IReadOnlyList<CompanyTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );
}
