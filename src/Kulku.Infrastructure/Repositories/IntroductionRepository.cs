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
            .Select(i => new
            {
                i.AvatarUrl,
                i.SmallAvatarUrl,
                Translation = i
                    .Translations.Where(t => t.Language == language)
                    .Select(t => new
                    {
                        t.Title,
                        t.Content,
                        t.Tagline,
                    })
                    .FirstOrDefault(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        return result is null
            ? null
            : new IntroductionResponse(
                Title: result.Translation?.Title ?? string.Empty,
                Content: result.Translation?.Content ?? string.Empty,
                Tagline: result.Translation?.Tagline ?? string.Empty,
                AvatarUrl: result.AvatarUrl,
                SmallAvatarUrl: result.SmallAvatarUrl
            );
    }
}
