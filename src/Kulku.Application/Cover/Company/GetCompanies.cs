using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Cover.Company;

/// <summary>
/// Lists all companies with translations for admin management.
/// </summary>
public static class GetCompanies
{
    public sealed record Query() : IQuery<IReadOnlyList<CompanyTranslationsModel>>;

    internal sealed class Handler(ICompanyQueries queries)
        : IQueryHandler<Query, IReadOnlyList<CompanyTranslationsModel>>
    {
        private readonly ICompanyQueries _queries = queries;

        public async Task<Result<IReadOnlyList<CompanyTranslationsModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllWithTranslationsAsync(cancellationToken));
        }
    }
}
