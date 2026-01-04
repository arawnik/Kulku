using Kulku.Application.Projects.Models;
using Kulku.Domain;
using Kulku.Domain.Projects;

namespace Kulku.Application.Projects.Ports;

public interface IKeywordQueries
{
    Task<KeywordModel?> FindByIdAsync(
        Guid id,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<KeywordModel>> ListByTypeAsync(
        KeywordType type,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
