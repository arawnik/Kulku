using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Gets a single proficiency level with translations for editing.
/// </summary>
public static class GetProficiencyDetail
{
    public sealed record Query(Guid ProficiencyId) : IQuery<ProficiencyTranslationsModel?>;

    internal sealed class Handler(IProficiencyQueries queries)
        : IQueryHandler<Query, ProficiencyTranslationsModel?>
    {
        private readonly IProficiencyQueries _queries = queries;

        public async Task<Result<ProficiencyTranslationsModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.FindByIdWithTranslationsAsync(query.ProficiencyId, cancellationToken)
            );
        }
    }
}
