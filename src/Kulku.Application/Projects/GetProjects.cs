using Kulku.Contract.Projects;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

public static class GetProjects
{
    public sealed record Query() : IQuery<ICollection<ProjectResponse>>;

    internal sealed class Handler(IProjectRepository repository)
        : IQueryHandler<Query, ICollection<ProjectResponse>>
    {
        private readonly IProjectRepository _repository = repository;

        public async Task<Result<ICollection<ProjectResponse>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _repository.QueryAllAsync(cancellationToken));
        }
    }
}
