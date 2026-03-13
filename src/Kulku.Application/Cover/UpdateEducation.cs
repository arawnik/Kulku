using Kulku.Domain;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover;

public static class UpdateEducation
{
    public sealed record Command(
        Guid EducationId,
        DateOnly StartDate,
        DateOnly? EndDate,
        IReadOnlyList<EducationTranslationDto> Translations
    ) : ICommand;

    public sealed record EducationTranslationDto(
        LanguageCode Language,
        string Title,
        string Description
    );

    internal sealed class Handler(IEducationRepository educationRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IEducationRepository _educationRepository = educationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = Validate(command);
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var education = await _educationRepository.GetByIdAsync(
                command.EducationId,
                cancellationToken
            );

            if (education is null)
                return Error.NotFound("Education not found.");

            education.StartDate = command.StartDate;
            education.EndDate = command.EndDate;

            MergeTranslations(education, command.Translations);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static Error[] Validate(Command command)
        {
            List<Error> errors = [];

            if (command.Translations.Count == 0)
                errors.Add(Error.BusinessRule("At least one translation is required."));

            if (command.EndDate.HasValue && command.EndDate < command.StartDate)
                errors.Add(Error.BusinessRule("End date cannot be before start date."));

            foreach (var t in command.Translations)
            {
                if (string.IsNullOrWhiteSpace(t.Title))
                    errors.Add(
                        Error.BusinessRule($"Title is required for the {t.Language} translation.")
                    );
            }

            return [.. errors];
        }

        private static void MergeTranslations(
            Education education,
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
