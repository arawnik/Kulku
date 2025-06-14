using Kulku.Contract.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover;

public static class GetIntroduction
{
    public sealed record Query() : IQuery<IntroductionResponse?>;

    internal sealed class Handler(IIntroductionRepository repository)
        : IQueryHandler<Query, IntroductionResponse?>
    {
        private readonly IIntroductionRepository _repository = repository;

        public async Task<Result<IntroductionResponse?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _repository.QueryCurrentAsync(cancellationToken));
        }
    }
}
