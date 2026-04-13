using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Company;

/// <summary>
/// Removes a company from network tracking (deletes its profile, cascading to categories).
/// Contacts and interactions are preserved but lose their profile link.
/// </summary>
public static class DisenrollNetworkCompany
{
    public sealed record Command(Guid CompanyId) : ICommand;

    internal sealed class Handler(
        ICompanyNetworkProfileRepository profileRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly ICompanyNetworkProfileRepository _profileRepository = profileRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.GetByIdAsync(
                command.CompanyId,
                cancellationToken
            );

            if (profile is null)
                return Error.NotFound("Network profile not found.");

            _profileRepository.Remove(profile);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
