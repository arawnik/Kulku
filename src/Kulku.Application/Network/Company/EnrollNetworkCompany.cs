using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Company;

/// <summary>
/// Enrolls an existing company in network tracking by creating a profile.
/// </summary>
public static class EnrollNetworkCompany
{
    public sealed record Command(
        Guid CompanyId,
        CompanyStage Stage,
        string? Notes,
        IReadOnlyList<Guid> CategoryIds
    ) : ICommand<Guid>;

    internal sealed class Handler(
        ICompanyNetworkProfileRepository profileRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, Guid>
    {
        private readonly ICompanyNetworkProfileRepository _profileRepository = profileRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var profile = new CompanyNetworkProfile
            {
                CompanyId = command.CompanyId,
                Stage = command.Stage,
                Notes = command.Notes?.Trim(),
                CompanyNetworkProfileCategories =
                [
                    .. command.CategoryIds.Select(catId => new CompanyNetworkProfileCategory
                    {
                        NetworkCategoryId = catId,
                    }),
                ],
            };

            _profileRepository.Add(profile);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(profile.Id);
        }
    }
}
