using Kulku.Application.Projects.Models;
using Kulku.Domain;

namespace Kulku.Application.Projects.Ports;

public interface IProjectQueries
{
    Task<IReadOnlyList<ProjectModel>> ListAllAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
