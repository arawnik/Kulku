using Kulku.Application.Resources;
using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Updates an existing keyword and merges its translations.
/// </summary>
public static class UpdateKeyword
{
    public sealed record Command(
        Guid KeywordId,
        KeywordType Type,
        Guid ProficiencyId,
        int Order,
        bool Display,
        IReadOnlyList<KeywordTranslationDto> Translations
    ) : ICommand;

    internal sealed class Handler(IKeywordRepository keywordRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IKeywordRepository _keywordRepository = keywordRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = KeywordCommandValidator.Validate(
                command.Type,
                command.ProficiencyId,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var keyword = await _keywordRepository.GetByIdAsync(
                command.KeywordId,
                cancellationToken
            );

            if (keyword is null)
                return Error.NotFound(Strings.NotFound_Keyword);

            keyword.Type = command.Type;
            keyword.ProficiencyId = command.ProficiencyId;
            keyword.Order = command.Order;
            keyword.Display = command.Display;

            MergeTranslations(keyword, command.Translations);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static void MergeTranslations(
            Keyword keyword,
            IReadOnlyList<KeywordTranslationDto> incoming
        )
        {
            var existing = keyword.Translations.ToDictionary(t => t.Language);
            keyword.Translations.Clear();

            foreach (var dto in incoming)
            {
                if (existing.TryGetValue(dto.Language, out var translation))
                {
                    translation.Name = dto.Name;
                    keyword.Translations.Add(translation);
                }
                else
                {
                    keyword.Translations.Add(
                        new KeywordTranslation { Language = dto.Language, Name = dto.Name }
                    );
                }
            }
        }
    }
}
