using Kulku.Application.IdeaBank.Models;
using Kulku.Domain;

namespace Kulku.Application.IdeaBank.Ports;

/// <summary>
/// Read-side queries for ideas.
/// </summary>
public interface IIdeaQueries
{
    /// <summary>
    /// Lists all ideas with lightweight data for the list view.
    /// </summary>
    Task<IReadOnlyList<IdeaListModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves a single idea with full detail including notes and tags.
    /// </summary>
    Task<IdeaDetailModel?> FindByIdAsync(
        Guid id,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
