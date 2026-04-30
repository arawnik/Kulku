using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Resources;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;
using DomainEducation = Kulku.Domain.Cover.Education;

namespace Kulku.Application.Cover.Education;

/// <summary>
/// Updates an existing education entry and merges its translations.
/// </summary>
public static class UpdateEducation
{
    /// <summary>
    /// Command to update an existing education entry.
    /// </summary>
    /// <param name="EducationId">The education entry to update.</param>
    /// <param name="StartDate">Updated start date.</param>
    /// <param name="EndDate">Updated end date, or <c>null</c> if ongoing.</param>
    /// <param name="Translations">Full set of translations to replace existing ones.</param>
    public sealed record Command(
        Guid EducationId,
        DateOnly StartDate,
        DateOnly? EndDate,
        IReadOnlyList<EducationTranslationDto> Translations
    ) : ICommand;

    internal sealed class Handler(IEducationRepository educationRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IEducationRepository _educationRepository = educationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = EducationCommandValidator.Validate(
                command.StartDate,
                command.EndDate,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var education = await _educationRepository.GetByIdAsync(
                command.EducationId,
                cancellationToken
            );

            if (education is null)
                return Error.NotFound(Strings.NotFound_Education);

            education.StartDate = command.StartDate;
            education.EndDate = command.EndDate;

            MergeTranslations(education, command.Translations);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static void MergeTranslations(
            DomainEducation education,
            IReadOnlyList<EducationTranslationDto> incoming
        )
        {
            var existing = education.Translations.ToDictionary(t => t.Language);
            education.Translations.Clear();

            foreach (var dto in incoming)
            {
                if (existing.TryGetValue(dto.Language, out var translation))
                {
                    translation.Title = dto.Title;
                    translation.Description = dto.Description;
                    education.Translations.Add(translation);
                }
                else
                {
                    education.Translations.Add(
                        new EducationTranslation
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
