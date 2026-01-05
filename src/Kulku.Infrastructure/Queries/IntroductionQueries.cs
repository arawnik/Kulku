using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

public class IntroductionQueries(AppDbContext context) : IIntroductionQueries
{
    private readonly AppDbContext _context = context;

    public async Task<IntroductionModel?> FindCurrentAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Introductions.Where(i => i.PubDate <= DateTime.UtcNow)
            .OrderBy(i => i.PubDate)
            .LeftJoin(
                _context.IntroductionTranslations.Where(t => t.Language == language),
                i => i.Id,
                t => t.IntroductionId,
                (i, it) =>
                    new IntroductionModel(
                        Title: it != null ? it.Title : string.Empty,
                        Content: it != null ? it.Content : string.Empty,
                        Tagline: it != null ? it.Tagline : string.Empty,
                        AvatarUrl: i.AvatarUrl,
                        SmallAvatarUrl: i.SmallAvatarUrl
                    )
            )
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
