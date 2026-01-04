using Kulku.Application.Cover.Models;
using Kulku.Domain;

namespace Kulku.Application.Cover.Ports;

public interface IEducationQueries
{
    Task<IReadOnlyList<EducationModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
