using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Experience.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Experience;

/// <summary>
/// Retrieves a single experience entry with all its translations.
/// </summary>
public static class GetExperienceDetail
{
    /// <summary>
    /// Query to find an experience entry by its identifier, including all translations.
    /// </summary>
    public sealed record Query(Guid ExperienceId) : IQuery<ExperienceTranslationsModel?>;

    internal sealed class Handler(IExperienceQueries queries)
        : IQueryHandler<Query, ExperienceTranslationsModel?>
    {
        private readonly IExperienceQueries _queries = queries;

        public async Task<Result<ExperienceTranslationsModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            var experience = await _queries.FindByIdWithTranslationsAsync(
                query.ExperienceId,
                cancellationToken
            );
            return Result.Success(experience);
        }
    }
}
