using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Network;

/// <summary>
/// EF Core implementation of <see cref="INetworkCompanyQueries"/>.
/// </summary>
public class NetworkCompanyQueries(AppDbContext context) : INetworkCompanyQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<NetworkCompanyModel>> ListEnrolledAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .CompanyNetworkProfiles.AsNoTracking()
            .LeftJoin(
                _context.CompanyTranslations.Where(t => t.Language == language),
                p => p.CompanyId,
                ct => ct.CompanyId,
                (p, ct) => new { p, ct }
            )
            .LeftJoin(
                _context.Companies,
                x => x.p.CompanyId,
                c => c.Id,
                (x, c) =>
                    new
                    {
                        x.p,
                        x.ct,
                        c,
                    }
            )
            .Select(x => new NetworkCompanyModel(
                CompanyId: x.p.CompanyId,
                Name: x.ct != null ? x.ct.Name : "Unknown",
                Website: x.c != null ? x.c.Website : null,
                Region: x.c != null ? x.c.Region : null,
                Stage: x.p.Stage,
                Notes: x.p.Notes,
                Categories: x.p.CompanyNetworkProfileCategories.Select(
                        pc => new NetworkCategoryModel(
                            pc.NetworkCategory.Id,
                            pc.NetworkCategory.Name,
                            pc.NetworkCategory.ColorToken
                        )
                    )
                    .ToList(),
                ContactCount: _context.NetworkContacts.Count(c => c.CompanyId == x.p.CompanyId),
                InteractionCount: _context.NetworkInteractions.Count(i =>
                    i.CompanyId == x.p.CompanyId
                )
            ))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<NetworkCompanyDetailModel?> FindByCompanyIdAsync(
        Guid companyId,
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .CompanyNetworkProfiles.AsNoTracking()
            .Where(p => p.CompanyId == companyId)
            .LeftJoin(
                _context.CompanyTranslations.Where(t => t.Language == language),
                p => p.CompanyId,
                ct => ct.CompanyId,
                (p, ct) => new { p, ct }
            )
            .LeftJoin(
                _context.Companies,
                x => x.p.CompanyId,
                c => c.Id,
                (x, c) =>
                    new
                    {
                        x.p,
                        x.ct,
                        c,
                    }
            )
            .Select(x => new NetworkCompanyDetailModel(
                CompanyId: x.p.CompanyId,
                Name: x.ct != null ? x.ct.Name : "Unknown",
                Website: x.c != null ? x.c.Website : null,
                Region: x.c != null ? x.c.Region : null,
                Stage: x.p.Stage,
                Notes: x.p.Notes,
                Categories: x.p.CompanyNetworkProfileCategories.Select(
                        pc => new NetworkCategoryModel(
                            pc.NetworkCategory.Id,
                            pc.NetworkCategory.Name,
                            pc.NetworkCategory.ColorToken
                        )
                    )
                    .ToList(),
                ContactCount: _context.NetworkContacts.Count(c => c.CompanyId == x.p.CompanyId),
                InteractionCount: _context.NetworkInteractions.Count(i =>
                    i.CompanyId == x.p.CompanyId
                ),
                LatestInteractionDate: _context
                    .NetworkInteractions.Where(i => i.CompanyId == x.p.CompanyId)
                    .OrderByDescending(i => i.Date)
                    .Select(i => (DateTime?)i.Date)
                    .FirstOrDefault(),
                PrimaryContactName: _context
                    .NetworkContacts.Where(c => c.CompanyId == x.p.CompanyId)
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => c.PersonName)
                    .FirstOrDefault()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<NetworkAvailableCompanyModel>> ListAvailableAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var enrolledIds = _context.CompanyNetworkProfiles.Select(p => p.CompanyId);

        return await _context
            .Companies.AsNoTracking()
            .Where(c => !enrolledIds.Contains(c.Id))
            .LeftJoin(
                _context.CompanyTranslations.Where(t => t.Language == language),
                c => c.Id,
                ct => ct.CompanyId,
                (c, ct) => new { c, ct }
            )
            .Select(x => new NetworkAvailableCompanyModel(
                CompanyId: x.c.Id,
                Name: x.ct != null ? x.ct.Name : "Unknown",
                Website: x.c.Website,
                Region: x.c.Region
            ))
            .ToListAsync(cancellationToken);
    }
}
