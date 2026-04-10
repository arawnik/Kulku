using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Interaction;

/// <summary>
/// Updates an existing network interaction.
/// </summary>
public static class UpdateNetworkInteraction
{
    public sealed record Command(
        Guid InteractionId,
        DateTime Date,
        InteractionDirection Direction,
        InteractionChannel Channel,
        bool IsWarmIntro,
        string? ReferredByName,
        string? ReferredByRelation,
        Guid? ContactId,
        string Summary,
        string? NextAction,
        DateTime? NextActionDue
    ) : ICommand;

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
            var errors = NetworkInteractionCommandValidator.Validate(
                command.Summary,
                command.IsWarmIntro,
                command.ReferredByName
            );
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var interaction = await _interactionRepository.GetByIdAsync(
                command.InteractionId,
                cancellationToken
            );

            if (interaction is null)
                return Error.NotFound("Interaction not found.");

            interaction.Date = command.Date;
            interaction.Direction = command.Direction;
            interaction.Channel = command.Channel;
            interaction.IsWarmIntro = command.IsWarmIntro;
            interaction.ReferredByName = command.IsWarmIntro
                ? command.ReferredByName?.Trim()
                : null;
            interaction.ReferredByRelation = command.IsWarmIntro
                ? command.ReferredByRelation?.Trim()
                : null;
            interaction.ContactId = command.ContactId;
            interaction.Summary = command.Summary.Trim();
            interaction.NextAction = string.IsNullOrWhiteSpace(command.NextAction)
                ? null
                : command.NextAction.Trim();
            interaction.NextActionDue = command.NextActionDue;

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
