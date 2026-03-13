using Kulku.Domain;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover;

public static class CreateEducation
{
    public sealed record Command(
        Guid InstitutionId,
        DateOnly StartDate,
        DateOnly? EndDate,
        IReadOnlyList<EducationTranslationDto> Translations
    ) : ICommand<Guid>;

    public sealed record EducationTranslationDto(
        LanguageCode Language,
        string Title,
        string Description
    );

    internal sealed class Handler(IEducationRepository educationRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Guid>
    {
        private readonly IEducationRepository _educationRepository = educationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = Validate(command);
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var education = new Education
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
    }
}
