using Kulku.Application.IdeaBank.Models;

namespace Kulku.Application.IdeaBank.Ports;

/// <summary>
/// Read-side queries for idea tags.
/// </summary>
public interface IIdeaTagQueries
{
    /// <summary>
    /// Lists all tags with their idea reference counts.
    /// </summary>
    Task<IReadOnlyList<IdeaTagModel>> ListAllAsync(CancellationToken cancellationToken = default);
}
