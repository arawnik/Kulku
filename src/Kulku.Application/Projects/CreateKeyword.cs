using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Creates a new keyword with translations.
/// </summary>
public static class CreateKeyword
{
    public sealed record Command(
        KeywordType Type,
        Guid ProficiencyId,
        int Order,
        bool Display,
        IReadOnlyList<KeywordTranslationDto> Translations
    ) : ICommand<Guid>;

    internal sealed class Handler(IKeywordRepository keywordRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Guid>
    {
        private readonly IKeywordRepository _keywordRepository = keywordRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = KeywordCommandValidator.Validate(
                command.Type,
                command.ProficiencyId,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var keyword = new Keyword
            {
                Type = command.Type,
                ProficiencyId = command.ProficiencyId,
                Order = command.Order,
                Display = command.Display,
                Translations =
                [
                    .. command.Translations.Select(t => new KeywordTranslation
                    {
                        Language = t.Language,
                        Name = t.Name,
                    }),
                ],
            };

            _keywordRepository.Add(keyword);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(keyword.Id);
        }
    }
}
