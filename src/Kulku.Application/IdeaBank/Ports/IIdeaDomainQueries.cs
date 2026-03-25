using Kulku.Application.IdeaBank.Models;
using Kulku.Domain;

namespace Kulku.Application.IdeaBank.Ports;

/// <summary>
/// Read-side queries for idea domains (seeded categories).
/// </summary>
public interface IIdeaDomainQueries
{
    /// <summary>
    /// Lists all idea domains ordered by display order, with names resolved for the given language.
    /// </summary>
    Task<IReadOnlyList<IdeaDomainModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
