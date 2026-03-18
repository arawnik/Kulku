using Kulku.Application.Cover.Introduction.Models;
using Kulku.Application.Cover.Introduction.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Introduction;

/// <summary>
/// Gets a single introduction with all its translations by ID.
/// </summary>
public static class GetIntroductionDetail
{
    public sealed record Query(Guid IntroductionId) : IQuery<IntroductionTranslationsModel?>;

    internal sealed class Handler(IIntroductionQueries queries)
        : IQueryHandler<Query, IntroductionTranslationsModel?>
    {
        private readonly IIntroductionQueries _queries = queries;

        public async Task<Result<IntroductionTranslationsModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.FindByIdWithTranslationsAsync(
                    query.IntroductionId,
                    cancellationToken
                )
            );
        }
    }
}
