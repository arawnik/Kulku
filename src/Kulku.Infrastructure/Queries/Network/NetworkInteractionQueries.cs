using Kulku.Application.Network.Models;
using Kulku.Application.Network.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries.Network;

/// <summary>
/// EF Core implementation of <see cref="INetworkInteractionQueries"/>.
/// </summary>
public class NetworkInteractionQueries(AppDbContext context) : INetworkInteractionQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<NetworkInteractionModel>> ListByCompanyAsync(
        Guid? companyId,
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.NetworkInteractions.AsNoTracking().AsQueryable();

        if (companyId.HasValue)
            query = query.Where(i => i.CompanyId == companyId.Value);

        return await query
            .OrderByDescending(i => i.Date)
            .LeftJoin(
                _context.CompanyTranslations.Where(t => t.Language == language),
                i => i.CompanyId,
                ct => ct.CompanyId,
                (i, ct) => new { i, ct }
            )
            .Select(x => new NetworkInteractionModel(
                Id: x.i.Id,
                CompanyId: x.i.CompanyId,
                CompanyName: x.ct != null ? x.ct.Name : "Unknown",
                ContactId: x.i.ContactId,
                ContactName: x.i.Contact != null ? x.i.Contact.PersonName : null,
                Date: x.i.Date,
                Direction: x.i.Direction,
                Channel: x.i.Channel,
                IsWarmIntro: x.i.IsWarmIntro,
                ReferredByName: x.i.ReferredByName,
                ReferredByRelation: x.i.ReferredByRelation,
                Summary: x.i.Summary,
                NextAction: x.i.NextAction,
                NextActionDue: x.i.NextActionDue,
                CreatedAt: x.i.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<NetworkInteractionModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .NetworkInteractions.AsNoTracking()
            .OrderByDescending(i => i.Date)
            .LeftJoin(
                _context.CompanyTranslations.Where(t => t.Language == language),
                i => i.CompanyId,
                ct => ct.CompanyId,
                (i, ct) => new { i, ct }
            )
            .Select(x => new NetworkInteractionModel(
                Id: x.i.Id,
                CompanyId: x.i.CompanyId,
                CompanyName: x.ct != null ? x.ct.Name : "Unknown",
                ContactId: x.i.ContactId,
                ContactName: x.i.Contact != null ? x.i.Contact.PersonName : null,
                Date: x.i.Date,
                Direction: x.i.Direction,
                Channel: x.i.Channel,
                IsWarmIntro: x.i.IsWarmIntro,
                ReferredByName: x.i.ReferredByName,
                ReferredByRelation: x.i.ReferredByRelation,
                Summary: x.i.Summary,
                NextAction: x.i.NextAction,
                NextActionDue: x.i.NextActionDue,
                CreatedAt: x.i.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<NetworkInteractionModel?> FindByIdAsync(
        Guid interactionId,
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .NetworkInteractions.AsNoTracking()
            .Where(i => i.Id == interactionId)
            .LeftJoin(
                _context.CompanyTranslations.Where(t => t.Language == language),
                i => i.CompanyId,
                ct => ct.CompanyId,
                (i, ct) => new { i, ct }
            )
            .Select(x => new NetworkInteractionModel(
                Id: x.i.Id,
                CompanyId: x.i.CompanyId,
                CompanyName: x.ct != null ? x.ct.Name : "Unknown",
                ContactId: x.i.ContactId,
                ContactName: x.i.Contact != null ? x.i.Contact.PersonName : null,
                Date: x.i.Date,
                Direction: x.i.Direction,
                Channel: x.i.Channel,
                IsWarmIntro: x.i.IsWarmIntro,
                ReferredByName: x.i.ReferredByName,
                ReferredByRelation: x.i.ReferredByRelation,
                Summary: x.i.Summary,
                NextAction: x.i.NextAction,
                NextActionDue: x.i.NextActionDue,
                CreatedAt: x.i.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
