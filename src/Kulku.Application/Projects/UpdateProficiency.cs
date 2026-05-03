using Kulku.Application.Resources;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Updates an existing proficiency level and merges its translations.
/// </summary>
public static class UpdateProficiency
{
    public sealed record Command(
        Guid ProficiencyId,
        int Scale,
        int Order,
        IReadOnlyList<ProficiencyTranslationDto> Translations
    ) : ICommand;

    internal sealed class Handler(
        IProficiencyRepository proficiencyRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IProficiencyRepository _proficiencyRepository = proficiencyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = ProficiencyCommandValidator.Validate(command.Scale, command.Translations);
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var proficiency = await _proficiencyRepository.GetByIdAsync(
                command.ProficiencyId,
                cancellationToken
            );

            if (proficiency is null)
                return Error.NotFound(Strings.NotFound_ProficiencyLevel);

            proficiency.Scale = command.Scale;
            proficiency.Order = command.Order;

            proficiency.MergeTranslations(
                command.Translations,
                (dto, t) =>
                {
                    t.Name = dto.Name;
                    t.Description = dto.Description;
                }
            );

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
