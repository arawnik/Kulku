using Kulku.Domain.Abstractions;
using Kulku.Domain.Cover;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="Experience"/> entities.
/// </summary>
public interface IExperienceRepository : IEntityRepository<Experience>
{
    /// <summary>
    /// Replaces the keyword associations for the given experience with the specified keyword IDs.
    /// The experience must be tracked and its <see cref="Experience.Keywords"/> collection loaded.
    /// </summary>
    Task SyncKeywordsAsync(
        Experience experience,
        IReadOnlyList<Guid> keywordIds,
        CancellationToken cancellationToken = default
    );
}
