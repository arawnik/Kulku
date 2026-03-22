using Kulku.Application.IdeaBank.Models;
using Kulku.Domain;

namespace Kulku.Application.IdeaBank.Ports;

/// <summary>
/// Read-side queries for idea priorities (seeded priority levels).
/// </summary>
public interface IIdeaPriorityQueries
{
    /// <summary>
    /// Lists all idea priorities ordered by display order, with names resolved for the given language.
    /// </summary>
    Task<IReadOnlyList<IdeaPriorityModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
