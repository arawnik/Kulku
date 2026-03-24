using Kulku.Application.Cover.Experience.Models;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;
using DomainExperience = Kulku.Domain.Cover.Experience;

namespace Kulku.Application.Cover.Experience;

/// <summary>
/// Creates a new experience entry with translations.
/// </summary>
public static class CreateExperience
{
    /// <summary>
    /// Command to create a new experience entry.
    /// </summary>
    /// <param name="CompanyId">The company this experience belongs to.</param>
    /// <param name="StartDate">When the experience started.</param>
    /// <param name="EndDate">When the experience ended, or <c>null</c> if ongoing.</param>
    /// <param name="Translations">Localized title and description per language.</param>
    public sealed record Command(
        Guid CompanyId,
        DateOnly StartDate,
        DateOnly? EndDate,
        IReadOnlyList<ExperienceTranslationDto> Translations
    ) : ICommand<Guid>;

    internal sealed class Handler(
        IExperienceRepository experienceRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, Guid>
    {
        private readonly IExperienceRepository _experienceRepository = experienceRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = ExperienceCommandValidator.Validate(
                command.StartDate,
                command.EndDate,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var experience = new DomainExperience
            {
                CompanyId = command.CompanyId,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
                Translations =
                [
                    .. command.Translations.Select(t => new ExperienceTranslation
                    {
                        Language = t.Language,
                        Title = t.Title,
                        Description = t.Description,
                    }),
                ],
            };

            _experienceRepository.Add(experience);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(experience.Id);
        }
    }
}
