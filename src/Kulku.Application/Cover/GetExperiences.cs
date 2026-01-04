using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover;

public static class GetExperiences
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<ExperienceModel>>,
            ILocalizedRequest;

    internal sealed class Handler(IExperienceQueries queries)
        : IQueryHandler<Query, IReadOnlyList<ExperienceModel>>
    {
        private readonly IExperienceQueries _queries = queries;

        public async Task<Result<IReadOnlyList<ExperienceModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(query.Language, cancellationToken));
        }
    }
}
