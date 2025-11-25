using Kulku.Contract.Cover;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using Kulku.Infrastructure.Helpers;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for accessing introduction and their full localization graph.
/// </summary>
public class IntroductionRepository(AppDbContext context) : IIntroductionRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Introduction?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Introductions.Where(i => i.Id == id)
            .Include(i => i.Translations)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(Introduction introduction)
    {
        _context.Introductions.Add(introduction);
    }

    public void Remove(Introduction introduction)
    {
        _context.Introductions.Remove(introduction);
    }

    public async Task<IntroductionResponse?> QueryCurrentAsync(
        CancellationToken cancellationToken = default
    )
    {
        var language = CultureLanguageHelper.GetCurrentLanguage();

        var result = await _context
            .Introductions.Where(i => i.PubDate <= DateTime.UtcNow)
            .OrderBy(i => i.PubDate)
            .LeftJoin(
                _context.IntroductionTranslations.Where(t => t.Language == language),
                i => i.Id,
                t => t.IntroductionId,
                (i, it) =>
                    new IntroductionResponse(
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
