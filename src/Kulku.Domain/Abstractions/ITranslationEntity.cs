using Kulku.Contract.Enums;

namespace Kulku.Domain.Abstractions;

/// <summary>
/// Marker interface for all translation entities.
/// Used for shared constraints or processing logic.
/// </summary>
public interface ITranslationEntity
{
    public LanguageCode Language { get; set; }
}
