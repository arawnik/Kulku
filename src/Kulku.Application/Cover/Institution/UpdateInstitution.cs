using Kulku.Application.Cover.Institution.Models;
using Kulku.Application.Resources;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Institution;

/// <summary>
/// Updates an existing institution and merges its translations.
/// </summary>
public static class UpdateInstitution
{
    public sealed record Command(
        Guid InstitutionId,
        IReadOnlyList<InstitutionTranslationDto> Translations
    ) : ICommand;

    internal sealed class Handler(
        IInstitutionRepository institutionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IInstitutionRepository _institutionRepository = institutionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = InstitutionCommandValidator.Validate(command.Translations);
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var institution = await _institutionRepository.GetByIdAsync(
                command.InstitutionId,
                cancellationToken
            );

            if (institution is null)
                return Error.NotFound(Strings.NotFound_Institution);

            institution.MergeTranslations(
                command.Translations,
                (dto, t) =>
                {
                    t.Name = dto.Name;
                    t.Department = dto.Department;
                    t.Description = dto.Description;
                }
            );

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
