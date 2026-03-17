using Kulku.Application.Projects.Models;
using Kulku.Domain;
using Kulku.Domain.Projects;

namespace Kulku.Application.Projects.Ports;

/// <summary>
/// Read-side port for keyword queries.
/// </summary>
public interface IKeywordQueries
{
    /// <summary>
    /// Finds a keyword by ID with its translation for the given language.
    /// </summary>
    Task<KeywordModel?> FindByIdAsync(
        Guid id,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns all keywords of a given type with translations for the given language.
    /// </summary>
    Task<IReadOnlyList<KeywordModel>> ListByTypeAsync(
        KeywordType type,
        LanguageCode language,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Returns all keywords as lightweight picker models for admin forms.
    /// Uses the first available translation for the name.
    /// </summary>
    Task<IReadOnlyList<KeywordPickerModel>> ListAllForPickerAsync(
        CancellationToken cancellationToken = default
    );
}
