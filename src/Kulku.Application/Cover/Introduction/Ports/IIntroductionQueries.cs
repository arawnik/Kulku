using Kulku.Application.Cover.Models;
using Kulku.Domain;

namespace Kulku.Application.Cover.Introduction.Ports;

public interface IIntroductionQueries
{
    Task<IntroductionModel?> FindCurrentAsync(
        LanguageCode language,
        CancellationToken cancellationToken = default
    );
}
