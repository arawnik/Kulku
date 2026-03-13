using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Experience.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Experience;

public static class GetExperienceTranslations
{
    public sealed record Query() : IQuery<IReadOnlyList<ExperienceTranslationsModel>>;

    internal sealed class Handler(IExperienceQueries queries)
        : IQueryHandler<Query, IReadOnlyList<ExperienceTranslationsModel>>
    {
        private readonly IExperienceQueries _queries = queries;

        public async Task<Result<IReadOnlyList<ExperienceTranslationsModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllWithTranslationsAsync(cancellationToken));
        }
    }
}
