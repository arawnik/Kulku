using Kulku.Application.Network.Models;
using Kulku.Domain;

namespace Kulku.Application.Network.Ports;

/// <summary>
/// Read-side port for network interaction queries.
/// </summary>
public interface INetworkInteractionQueries
{
    /// <summary>
    /// Returns interactions for a specific company ordered by date descending,
    /// or all interactions when companyId is null.
    /// </summary>
    Task<IReadOnlyList<NetworkInteractionModel>> ListByCompanyAsync(
        Guid? companyId,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns all interactions ordered by date descending.
    /// </summary>
    Task<IReadOnlyList<NetworkInteractionModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns a single interaction with company and contact names, or null if not found.
    /// </summary>
    Task<NetworkInteractionModel?> FindByIdAsync(
        Guid interactionId,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
