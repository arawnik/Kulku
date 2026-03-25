using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Cover.Education.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Education;

/// <summary>
/// Retrieves a single education entry with all its translations.
/// </summary>
public static class GetEducationDetail
{
    /// <summary>
    /// Query to find an education entry by its identifier, including all translations.
    /// </summary>
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
