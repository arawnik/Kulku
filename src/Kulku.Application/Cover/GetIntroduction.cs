using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover;

public static class GetIntroduction
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IntroductionModel?>,
            ILocalizedRequest;

    internal sealed class Handler(IIntroductionQueries queries)
        : IQueryHandler<Query, IntroductionModel?>
    {
        private readonly IIntroductionQueries _queries = queries;

        public async Task<Result<IntroductionModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.FindCurrentAsync(query.Language, cancellationToken)
            );
        }
    }
}
