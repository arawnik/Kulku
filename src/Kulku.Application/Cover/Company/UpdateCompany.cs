using Kulku.Application.Cover.Company.Models;
using Kulku.Application.Resources;
using Kulku.Domain.Abstractions;
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
    public sealed record Command(
        Guid CompanyId,
        string? Website,
        string? Region,
        IReadOnlyList<CompanyTranslationDto> Translations
    ) : ICommand;

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
                return Error.NotFound(Strings.NotFound_Company);

            company.Website = command.Website;
            company.Region = command.Region;

            company.MergeTranslations(
                command.Translations,
                (dto, t) =>
                {
                    t.Name = dto.Name;
                    t.Description = dto.Description;
                }
            );

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
