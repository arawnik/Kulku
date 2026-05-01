using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Experience;

/// <summary>
/// Deletes an experience entry and its associated translations.
/// </summary>
public static class DeleteExperience
{
    /// <summary>
    /// Command to delete an experience entry.
    /// </summary>
    /// <param name="ExperienceId">The experience entry to remove.</param>
    public sealed record Command(Guid ExperienceId) : ICommand;

    internal sealed class Handler(
        IExperienceRepository experienceRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IExperienceRepository _experienceRepository = experienceRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var experience = await _experienceRepository.GetByIdAsync(
                command.ExperienceId,
                cancellationToken
            );

            if (experience is null)
                return Error.NotFound(Strings.NotFound_Experience);

            _experienceRepository.Remove(experience);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
