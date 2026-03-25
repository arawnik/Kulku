using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Institution;

/// <summary>
/// Lists all institutions with translations for admin management.
/// </summary>
public static class GetInstitutions
{
    public sealed record Query() : IQuery<IReadOnlyList<InstitutionTranslationsModel>>;

    internal sealed class Handler(IInstitutionQueries queries)
        : IQueryHandler<Query, IReadOnlyList<InstitutionTranslationsModel>>
    {
        private readonly IInstitutionQueries _queries = queries;

        public async Task<Result<IReadOnlyList<InstitutionTranslationsModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllWithTranslationsAsync(cancellationToken));
        }
    }
}
