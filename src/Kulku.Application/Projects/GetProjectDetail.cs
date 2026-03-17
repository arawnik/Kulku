using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Retrieves a single project with all translations and keyword IDs for editing.
/// </summary>
public static class GetProjectDetail
{
    /// <param name="ProjectId">The project to retrieve.</param>
    public sealed record Query(Guid ProjectId) : IQuery<ProjectTranslationsModel?>;

    internal sealed class Handler(IProjectQueries queries)
        : IQueryHandler<Query, ProjectTranslationsModel?>
    {
        private readonly IProjectQueries _queries = queries;

        public async Task<Result<ProjectTranslationsModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        ) =>
            Result.Success(
                await _queries.FindByIdWithTranslationsAsync(query.ProjectId, cancellationToken)
            );
    }
}
