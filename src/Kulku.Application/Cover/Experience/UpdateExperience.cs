using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Resources;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;
using DomainExperience = Kulku.Domain.Cover.Experience;

namespace Kulku.Application.Cover.Experience;

/// <summary>
/// Updates an existing experience entry and merges its translations.
/// </summary>
public static class UpdateExperience
{
    /// <summary>
    /// Command to update an existing experience entry.
    /// </summary>
    /// <param name="ExperienceId">The experience entry to update.</param>
    /// <param name="StartDate">Updated start date.</param>
    /// <param name="EndDate">Updated end date, or <c>null</c> if ongoing.</param>
    /// <param name="Translations">Full set of translations to replace existing ones.</param>
    public sealed record Command(
        Guid ExperienceId,
        DateOnly StartDate,
        DateOnly? EndDate,
        IReadOnlyList<ExperienceTranslationDto> Translations
    ) : ICommand;

    internal sealed class Handler(
        IExperienceRepository experienceRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IExperienceRepository _experienceRepository = experienceRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = ExperienceCommandValidator.Validate(
                command.StartDate,
                command.EndDate,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var experience = await _experienceRepository.GetByIdAsync(
                command.ExperienceId,
                cancellationToken
            );

            if (experience is null)
                return Error.NotFound(Strings.NotFound_Experience);

            experience.StartDate = command.StartDate;
            experience.EndDate = command.EndDate;

            MergeTranslations(experience, command.Translations);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static void MergeTranslations(
            DomainExperience experience,
            IReadOnlyList<ExperienceTranslationDto> incoming
        )
        {
            var existing = experience.Translations.ToDictionary(t => t.Language);
            experience.Translations.Clear();

            foreach (var dto in incoming)
            {
                if (existing.TryGetValue(dto.Language, out var translation))
                {
                    translation.Title = dto.Title;
                    translation.Description = dto.Description;
                    experience.Translations.Add(translation);
                }
                else
                {
                    experience.Translations.Add(
                        new ExperienceTranslation
                        {
                            Language = dto.Language,
                            Title = dto.Title,
                            Description = dto.Description,
                        }
                    );
                }
            }
        }
    }
}
