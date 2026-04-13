using Kulku.Application.Cover.Institution.Models;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;
using DomainInstitution = Kulku.Domain.Cover.Institution;

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
                return Error.NotFound("Institution not found.");

            MergeTranslations(institution, command.Translations);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static void MergeTranslations(
            DomainInstitution institution,
            IReadOnlyList<InstitutionTranslationDto> incoming
        )
        {
            var existing = institution.Translations.ToDictionary(t => t.Language);
            institution.Translations.Clear();

            foreach (var dto in incoming)
            {
                if (existing.TryGetValue(dto.Language, out var translation))
                {
                    translation.Name = dto.Name;
                    translation.Department = dto.Department;
                    translation.Description = dto.Description;
                    institution.Translations.Add(translation);
                }
                else
                {
                    institution.Translations.Add(
                        new InstitutionTranslation
                        {
                            Language = dto.Language,
                            Name = dto.Name,
                            Department = dto.Department,
                            Description = dto.Description,
                        }
                    );
                }
            }
        }
    }
}
