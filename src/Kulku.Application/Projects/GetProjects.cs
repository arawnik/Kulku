using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

public static class GetProjects
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<ProjectModel>>,
            ILocalizedRequest;

    internal sealed class Handler(IProjectQueries queries)
        : IQueryHandler<Query, IReadOnlyList<ProjectModel>>
    {
        private readonly IProjectQueries _queries = queries;

        public async Task<Result<IReadOnlyList<ProjectModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(query.Language, cancellationToken));
        }
    }
}
