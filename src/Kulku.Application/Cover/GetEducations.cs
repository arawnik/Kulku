using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover;

public static class GetEducations
{
    public sealed record Query(LanguageCode Language)
        : IQuery<IReadOnlyList<EducationModel>>,
            ILocalizedRequest;

    internal sealed class Handler(IEducationQueries queries)
        : IQueryHandler<Query, IReadOnlyList<EducationModel>>
    {
        private readonly IEducationQueries _queries = queries;

        public async Task<Result<IReadOnlyList<EducationModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllAsync(query.Language, cancellationToken));
        }
    }
}
