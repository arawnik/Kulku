using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Institution;

/// <summary>
/// Gets a single institution with translations for editing.
/// </summary>
public static class GetInstitutionDetail
{
    public sealed record Query(Guid InstitutionId) : IQuery<InstitutionTranslationsModel?>;

    internal sealed class Handler(IInstitutionQueries queries)
        : IQueryHandler<Query, InstitutionTranslationsModel?>
    {
        private readonly IInstitutionQueries _queries = queries;

        public async Task<Result<InstitutionTranslationsModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.FindByIdWithTranslationsAsync(query.InstitutionId, cancellationToken)
            );
        }
    }
}
