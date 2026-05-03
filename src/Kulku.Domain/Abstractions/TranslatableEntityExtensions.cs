namespace Kulku.Domain.Abstractions;

/// <summary>
/// Extension methods for <see cref="ITranslatableEntity{TTranslation}"/>
/// that provide reusable translation-merge logic.
/// </summary>
public static class TranslatableEntityExtensions
{
    /// <summary>
    /// Merges incoming translation DTOs into the entity's <see cref="ITranslatableEntity{TTranslation}.Translations"/> collection.
    /// Existing translations (matched by <see cref="ITranslationEntity.Language"/>) are updated in-place to preserve EF Core identity;
    /// new languages produce new translation entities.
    /// </summary>
    /// <typeparam name="TTranslation">The domain translation entity type.</typeparam>
    /// <typeparam name="TDto">The incoming DTO type that carries translated field values.</typeparam>
    /// <param name="entity">The translatable domain entity whose translations will be replaced.</param>
    /// <param name="incoming">The full set of translation DTOs that should exist after the merge.</param>
    /// <param name="apply">Copies field values from <typeparamref name="TDto"/> onto a <typeparamref name="TTranslation"/>. Called for both existing and newly created translations.</param>
    public static void MergeTranslations<TTranslation, TDto>(
        this ITranslatableEntity<TTranslation> entity,
        IReadOnlyList<TDto> incoming,
        Action<TDto, TTranslation> apply
    )
        where TTranslation : class, ITranslationEntity, new()
        where TDto : ITranslationDto
    {
        var existing = entity.Translations.ToDictionary(t => t.Language);
        entity.Translations.Clear();

        foreach (var dto in incoming)
        {
            if (existing.TryGetValue(dto.Language, out var translation))
            {
                apply(dto, translation);
                entity.Translations.Add(translation);
            }
            else
            {
                var newTranslation = new TTranslation { Language = dto.Language };
                apply(dto, newTranslation);
                entity.Translations.Add(newTranslation);
            }
        }
    }
}
