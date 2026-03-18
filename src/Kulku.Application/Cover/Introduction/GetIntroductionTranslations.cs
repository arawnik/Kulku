using Kulku.Application.Cover.Introduction.Models;
using Kulku.Application.Cover.Introduction.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Introduction;

/// <summary>
/// Lists all introductions with all their translations, sorted by PubDate descending.
/// </summary>
public static class GetIntroductionTranslations
{
    public sealed record Query() : IQuery<IReadOnlyList<IntroductionTranslationsModel>>;

    internal sealed class Handler(IIntroductionQueries queries)
        : IQueryHandler<Query, IReadOnlyList<IntroductionTranslationsModel>>
    {
        private readonly IIntroductionQueries _queries = queries;

        public async Task<Result<IReadOnlyList<IntroductionTranslationsModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllWithTranslationsAsync(cancellationToken));
        }
    }
}
