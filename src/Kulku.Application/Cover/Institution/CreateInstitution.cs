using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Institution;

/// <summary>
/// Creates a new institution with translations.
/// </summary>
public static class CreateInstitution
{
    public sealed record Command(IReadOnlyList<InstitutionTranslationDto> Translations)
        : ICommand<Guid>;

    internal sealed class Handler(
        IInstitutionRepository institutionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, Guid>
    {
        private readonly IInstitutionRepository _institutionRepository = institutionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = InstitutionCommandValidator.Validate(command.Translations);
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var institution = new Domain.Cover.Institution
            {
                Translations =
                [
                    .. command.Translations.Select(t => new InstitutionTranslation
                    {
                        Language = t.Language,
                        Name = t.Name,
                        Department = t.Department,
                        Description = t.Description,
                    }),
                ],
            };

            _institutionRepository.Add(institution);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(institution.Id);
        }
    }
}
