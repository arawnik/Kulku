using Kulku.Application.Cover.Introduction.Models;
using Kulku.Application.Cover.Introduction.Ports;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Infrastructure.Queries;

/// <summary>
/// EF Core implementation of introduction read-side queries.
/// </summary>
public class IntroductionQueries(AppDbContext context) : IIntroductionQueries
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IntroductionModel?> FindCurrentAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Introductions.Where(i => i.PubDate <= DateTime.UtcNow)
            .OrderByDescending(i => i.PubDate)
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

    /// <inheritdoc />
    public async Task<IReadOnlyList<IntroductionTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Introductions.AsNoTracking()
            .OrderByDescending(i => i.PubDate)
            .Select(i => new IntroductionTranslationsModel(
                IntroductionId: i.Id,
                AvatarUrl: i.AvatarUrl,
                SmallAvatarUrl: i.SmallAvatarUrl,
                PubDate: i.PubDate,
                Translations: i.Translations.Select(t => new IntroductionTranslationItem(
                        t.Language,
                        t.Title,
                        t.Tagline,
                        t.Content
                    ))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public async Task<IntroductionTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid introductionId,
        CancellationToken cancellationToken = default
    )
    {
        var result = await _context
            .Introductions.AsNoTracking()
            .Where(i => i.Id == introductionId)
            .Select(i => new IntroductionTranslationsModel(
                IntroductionId: i.Id,
                AvatarUrl: i.AvatarUrl,
                SmallAvatarUrl: i.SmallAvatarUrl,
                PubDate: i.PubDate,
                Translations: i.Translations.Select(t => new IntroductionTranslationItem(
                        t.Language,
                        t.Title,
                        t.Tagline,
                        t.Content
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}
