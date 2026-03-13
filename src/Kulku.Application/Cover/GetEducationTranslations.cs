using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover;

public static class GetEducationTranslations
{
    public sealed record Query() : IQuery<IReadOnlyList<EducationTranslationsModel>>;

    internal sealed class Handler(IEducationQueries queries)
        : IQueryHandler<Query, IReadOnlyList<EducationTranslationsModel>>
    {
        private readonly IEducationQueries _queries = queries;

        public async Task<Result<IReadOnlyList<EducationTranslationsModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllWithTranslationsAsync(cancellationToken));
        }
    }
}
