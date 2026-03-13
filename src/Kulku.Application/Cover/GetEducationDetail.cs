using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover;

public static class GetEducationDetail
{
    public sealed record Query(Guid EducationId) : IQuery<EducationTranslationsModel?>;

    internal sealed class Handler(IEducationQueries queries)
        : IQueryHandler<Query, EducationTranslationsModel?>
    {
        private readonly IEducationQueries _queries = queries;

        public async Task<Result<EducationTranslationsModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            var education = await _queries.FindByIdWithTranslationsAsync(
                query.EducationId,
                cancellationToken
            );
            return Result.Success(education);
        }
    }
}
