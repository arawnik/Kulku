using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Cover.Education.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Education;

/// <summary>
/// Retrieves all education entries with every available translation.
/// Used by admin views that display and edit content in all languages.
/// </summary>
public static class GetEducationTranslations
{
    /// <summary>
    /// Query to list all education entries with their full translation sets.
    /// </summary>
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
