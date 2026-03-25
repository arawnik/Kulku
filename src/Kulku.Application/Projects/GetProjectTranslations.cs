using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Retrieves all projects with all translations for admin editing.
/// </summary>
public static class GetProjectTranslations
{
    public sealed record Query() : IQuery<IReadOnlyList<ProjectTranslationsModel>>;

    internal sealed class Handler(IProjectQueries queries)
        : IQueryHandler<Query, IReadOnlyList<ProjectTranslationsModel>>
    {
        private readonly IProjectQueries _queries = queries;

        public async Task<Result<IReadOnlyList<ProjectTranslationsModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        ) => Result.Success(await _queries.ListAllWithTranslationsAsync(cancellationToken));
    }
}
