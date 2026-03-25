using Kulku.Application.Cover.Introduction.Models;
using Kulku.Application.Cover.Models;
using Kulku.Domain;

namespace Kulku.Application.Cover.Introduction.Ports;

public interface IIntroductionQueries
{
    /// <summary>
    /// Finds the currently active introduction (most recent PubDate ≤ UtcNow)
    /// for the given language.
    /// </summary>
    Task<IntroductionModel?> FindCurrentAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Lists all introductions with all their translations, ordered by PubDate descending.
    /// Used by admin views.
    /// </summary>
    Task<IReadOnlyList<IntroductionTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds a single introduction with all its translations by ID.
    /// Used by admin edit views.
    /// </summary>
    Task<IntroductionTranslationsModel?> FindByIdWithTranslationsAsync(
        Guid introductionId,
        CancellationToken cancellationToken = default
    );
}
