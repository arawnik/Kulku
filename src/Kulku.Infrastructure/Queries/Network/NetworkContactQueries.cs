using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Network;

/// <summary>
/// EF Core implementation of <see cref="INetworkContactQueries"/>.
/// </summary>
public class NetworkContactQueries(AppDbContext context) : INetworkContactQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<NetworkContactModel>> ListByCompanyAsync(
        Guid? companyId,
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.NetworkContacts.AsNoTracking().AsQueryable();

        if (companyId.HasValue)
            query = query.Where(c => c.CompanyId == companyId.Value);

        return await query
            .OrderBy(c => c.PersonName)
            .LeftJoin(
                _context.CompanyTranslations.Where(t => t.Language == language),
                c => c.CompanyId,
                ct => ct.CompanyId,
                (c, ct) => new { c, ct }
            )
            .Select(x => new NetworkContactModel(
                Id: x.c.Id,
                CompanyId: x.c.CompanyId,
                CompanyName: x.ct != null ? x.ct.Name : null,
                PersonName: x.c.PersonName,
                Email: x.c.Email,
                Phone: x.c.Phone,
                LinkedInUrl: x.c.LinkedInUrl,
                Title: x.c.Title,
                CreatedAt: x.c.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<NetworkContactModel>> ListUnaffiliatedAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .NetworkContacts.AsNoTracking()
            .Where(c => c.CompanyId == null)
            .OrderBy(c => c.PersonName)
            .Select(c => new NetworkContactModel(
                Id: c.Id,
                CompanyId: null,
                CompanyName: null,
                PersonName: c.PersonName,
                Email: c.Email,
                Phone: c.Phone,
                LinkedInUrl: c.LinkedInUrl,
                Title: c.Title,
                CreatedAt: c.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}
