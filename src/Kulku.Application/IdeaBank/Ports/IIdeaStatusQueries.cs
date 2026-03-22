using Kulku.Application.IdeaBank.Models;
using Kulku.Domain;

namespace Kulku.Application.IdeaBank.Ports;

/// <summary>
/// Read-side queries for idea statuses (seeded lifecycle states).
/// </summary>
public interface IIdeaStatusQueries
{
    /// <summary>
    /// Lists all idea statuses ordered by display order, with names resolved for the given language.
    /// </summary>
    Task<IReadOnlyList<IdeaStatusModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
