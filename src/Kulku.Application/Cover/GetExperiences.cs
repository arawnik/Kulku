using Kulku.Contract.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover;

public static class GetExperiences
{
    public sealed record Query() : IQuery<ICollection<ExperienceResponse>>;

    internal sealed class Handler(IExperienceRepository repository)
        : IQueryHandler<Query, ICollection<ExperienceResponse>>
    {
        private readonly IExperienceRepository _repository = repository;

        public async Task<Result<ICollection<ExperienceResponse>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _repository.QueryAllAsync(cancellationToken));
        }
    }
}
