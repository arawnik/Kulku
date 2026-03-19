using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Company;

/// <summary>
/// Creates a new company with translations.
/// </summary>
public static class CreateCompany
{
    public sealed record Command(IReadOnlyList<CompanyTranslationDto> Translations)
        : ICommand<Guid>;

    internal sealed class Handler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Guid>
    {
        private readonly ICompanyRepository _companyRepository = companyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = CompanyCommandValidator.Validate(command.Translations);
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var company = new Domain.Cover.Company
            {
                Translations =
                [
                    .. command.Translations.Select(t => new CompanyTranslation
                    {
                        Language = t.Language,
                        Name = t.Name,
                        Description = t.Description,
                    }),
                ],
            };

            _companyRepository.Add(company);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(company.Id);
        }
    }
}
