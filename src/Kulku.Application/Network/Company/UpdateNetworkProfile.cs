using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Company;

/// <summary>
/// Updates an enrolled company's network profile (stage, notes, categories).
/// </summary>
public static class UpdateNetworkProfile
{
    public sealed record Command(
        Guid CompanyId,
        CompanyStage Stage,
        string? Notes,
        IReadOnlyList<Guid> CategoryIds
    ) : ICommand;

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

            profile.Stage = command.Stage;
            profile.Notes = command.Notes?.Trim();

            // Replace categories
            profile.CompanyNetworkProfileCategories.Clear();
            foreach (var catId in command.CategoryIds)
            {
                profile.CompanyNetworkProfileCategories.Add(
                    new CompanyNetworkProfileCategory { NetworkCategoryId = catId }
                );
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
