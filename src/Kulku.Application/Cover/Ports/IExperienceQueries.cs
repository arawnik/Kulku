using Kulku.Application.Cover.Models;
using Kulku.Domain;

namespace Kulku.Application.Cover.Ports;

public interface IExperienceQueries
{
    Task<IReadOnlyList<ExperienceModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<ExperienceTranslationsModel>> ListAllWithTranslationsAsync(
        CancellationToken cancellationToken = default
    );
}
