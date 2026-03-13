using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Models;
using Kulku.Domain;

namespace Kulku.Application.Cover.Experience.Ports;

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
