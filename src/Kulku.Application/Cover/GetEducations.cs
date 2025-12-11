using Kulku.Contract.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover;

public static class GetEducations
{
    public sealed record Query() : IQuery<ICollection<EducationResponse>>;

    internal sealed class Handler(IEducationRepository repository)
        : IQueryHandler<Query, ICollection<EducationResponse>>
    {
        private readonly IEducationRepository _repository = repository;

        public async Task<Result<ICollection<EducationResponse>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _repository.QueryAllAsync(cancellationToken));
        }
    }
}
