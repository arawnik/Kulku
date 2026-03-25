using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Company;

/// <summary>
/// Gets a single company with translations for editing.
/// </summary>
public static class GetCompanyDetail
{
    public sealed record Query(Guid CompanyId) : IQuery<CompanyTranslationsModel?>;

    internal sealed class Handler(ICompanyQueries queries)
        : IQueryHandler<Query, CompanyTranslationsModel?>
    {
        private readonly ICompanyQueries _queries = queries;

        public async Task<Result<CompanyTranslationsModel?>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(
                await _queries.FindByIdWithTranslationsAsync(query.CompanyId, cancellationToken)
            );
        }
    }
}
