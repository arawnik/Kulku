using Kulku.Application.Cover.Company.Models;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Company;

/// <summary>
/// Updates an existing company and merges its translations.
/// </summary>
public static class UpdateCompany
{
    public sealed record Command(Guid CompanyId, IReadOnlyList<CompanyTranslationDto> Translations)
        : ICommand;

    internal sealed class Handler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly ICompanyRepository _companyRepository = companyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = CompanyCommandValidator.Validate(command.Translations);
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var company = await _companyRepository.GetByIdAsync(
                command.CompanyId,
                cancellationToken
            );

            if (company is null)
                return Error.NotFound("Company not found.");

            MergeTranslations(company, command.Translations);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static void MergeTranslations(
            Domain.Cover.Company company,
            IReadOnlyList<CompanyTranslationDto> incoming
        )
        {
            var existing = company.Translations.ToDictionary(t => t.Language);
            company.Translations.Clear();

            foreach (var dto in incoming)
            {
                if (existing.TryGetValue(dto.Language, out var translation))
                {
                    translation.Name = dto.Name;
                    translation.Description = dto.Description;
                    company.Translations.Add(translation);
                }
                else
                {
                    company.Translations.Add(
                        new CompanyTranslation
                        {
                            Language = dto.Language,
                            Name = dto.Name,
                            Description = dto.Description,
                        }
                    );
                }
            }
        }
    }
}
