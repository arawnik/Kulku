using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Creates a new proficiency level with translations.
/// </summary>
public static class CreateProficiency
{
    public sealed record Command(
        int Scale,
        int Order,
        IReadOnlyList<ProficiencyTranslationDto> Translations
    ) : ICommand<Guid>;

    internal sealed class Handler(
        IProficiencyRepository proficiencyRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, Guid>
    {
        private readonly IProficiencyRepository _proficiencyRepository = proficiencyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = ProficiencyCommandValidator.Validate(command.Scale, command.Translations);
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var proficiency = new Proficiency
            {
                Scale = command.Scale,
                Order = command.Order,
                Translations =
                [
                    .. command.Translations.Select(t => new ProficiencyTranslation
                    {
                        Language = t.Language,
                        Name = t.Name,
                        Description = t.Description,
                    }),
                ],
            };

            _proficiencyRepository.Add(proficiency);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(proficiency.Id);
        }
    }
}
