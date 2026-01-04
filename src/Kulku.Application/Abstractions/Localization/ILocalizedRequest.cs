using Kulku.Domain;

namespace Kulku.Application.Abstractions.Localization;

/// <summary>
/// Marks a request as requiring a specific language context for localized processing.
/// </summary>
/// <remarks>
/// Requests implementing this interface explicitly carry a <see cref="LanguageCode"/> to indicate
/// which language should be used when resolving localized data.
/// </remarks>
public interface ILocalizedRequest
{
    /// <summary>
    /// Gets the language to be used for localized data resolution during request handling.
    /// </summary>
    LanguageCode Language { get; }
}
