using System.Globalization;
using Kulku.Application.Cover.Ports;
using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Company;

/// <summary>
/// Deletes a company. Fails if any experiences still reference it.
/// </summary>
public static class DeleteCompany
{
    public sealed record Command(Guid CompanyId) : ICommand;

    internal sealed class Handler(
        ICompanyRepository companyRepository,
        ICompanyQueries companyQueries,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly ICompanyRepository _companyRepository = companyRepository;
        private readonly ICompanyQueries _companyQueries = companyQueries;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(
                command.CompanyId,
                cancellationToken
            );

            if (company is null)
                return Error.NotFound(Strings.NotFound_Company);

            var detail = await _companyQueries.FindByIdWithTranslationsAsync(
                command.CompanyId,
                cancellationToken
            );

            if (detail is not null && detail.ExperienceCount > 0)
                return Error.Validation(
                    "companyId",
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Strings.CannotDelete_CompanyReferenced,
                        detail.ExperienceCount
                    )
                );

            _companyRepository.Remove(company);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
