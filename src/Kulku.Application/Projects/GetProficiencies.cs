using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Lists all proficiency levels with translations for admin management.
/// </summary>
public static class GetProficiencies
{
    public sealed record Query() : IQuery<IReadOnlyList<ProficiencyTranslationsModel>>;

    internal sealed class Handler(IProficiencyQueries queries)
        : IQueryHandler<Query, IReadOnlyList<ProficiencyTranslationsModel>>
    {
        private readonly IProficiencyQueries _queries = queries;

        public async Task<Result<IReadOnlyList<ProficiencyTranslationsModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllWithTranslationsAsync(cancellationToken));
        }
    }
}
