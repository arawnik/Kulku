using Kulku.Domain;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;
using DomainEducation = Kulku.Domain.Cover.Education;

namespace Kulku.Application.Cover.Education;

/// <summary>
/// Creates a new education entry with translations.
/// </summary>
public static class CreateEducation
{
    /// <summary>
    /// Command to create a new education entry.
    /// </summary>
    /// <param name="InstitutionId">The institution this education belongs to.</param>
    /// <param name="StartDate">When the education started.</param>
    /// <param name="EndDate">When the education ended, or <c>null</c> if ongoing.</param>
    /// <param name="Translations">Localized title and description per language.</param>
    public sealed record Command(
        Guid InstitutionId,
        DateOnly StartDate,
        DateOnly? EndDate,
        IReadOnlyList<EducationTranslationDto> Translations
    ) : ICommand<Guid>;

    internal sealed class Handler(IEducationRepository educationRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Guid>
    {
        private readonly IEducationRepository _educationRepository = educationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = EducationCommandValidator.Validate(
                command.StartDate,
                command.EndDate,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var education = new DomainEducation
            {
                InstitutionId = command.InstitutionId,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
                Translations =
                [
                    .. command.Translations.Select(t => new EducationTranslation
                    {
                        Language = t.Language,
                        Title = t.Title,
                        Description = t.Description,
                    }),
                ],
            };

            _educationRepository.Add(education);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(education.Id);
        }
    }
}
