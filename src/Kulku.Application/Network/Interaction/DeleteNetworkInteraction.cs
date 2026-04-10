using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Interaction;

/// <summary>
/// Deletes a network interaction.
/// </summary>
public static class DeleteNetworkInteraction
{
    public sealed record Command(Guid InteractionId) : ICommand;

    internal sealed class Handler(
        INetworkInteractionRepository interactionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly INetworkInteractionRepository _interactionRepository =
            interactionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var interaction = await _interactionRepository.GetByIdAsync(
                command.InteractionId,
                cancellationToken
            );

            if (interaction is null)
                return Error.NotFound("Interaction not found.");

            _interactionRepository.Remove(interaction);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
