using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Interaction;

/// <summary>
/// Records a new interaction with a company in the professional network.
/// </summary>
public static class CreateNetworkInteraction
{
    public sealed record Command(
        Guid CompanyId,
        Guid? ContactId,
        DateTime Date,
        InteractionDirection Direction,
        InteractionChannel Channel,
        bool IsWarmIntro,
        string? ReferredByName,
        string? ReferredByRelation,
        string Summary,
        string? NextAction,
        DateTime? NextActionDue
    ) : ICommand<Guid>;

    internal sealed class Handler(
        INetworkInteractionRepository interactionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, Guid>
    {
        private readonly INetworkInteractionRepository _interactionRepository =
            interactionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = NetworkInteractionCommandValidator.Validate(
                command.Summary,
                command.IsWarmIntro,
                command.ReferredByName
            );
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var interaction = new NetworkInteraction
            {
                CompanyId = command.CompanyId,
                ContactId = command.ContactId,
                Date = command.Date,
                Direction = command.Direction,
                Channel = command.Channel,
                IsWarmIntro = command.IsWarmIntro,
                ReferredByName = command.IsWarmIntro ? command.ReferredByName?.Trim() : null,
                ReferredByRelation = command.IsWarmIntro
                    ? command.ReferredByRelation?.Trim()
                    : null,
                Summary = command.Summary.Trim(),
                NextAction = string.IsNullOrWhiteSpace(command.NextAction)
                    ? null
                    : command.NextAction.Trim(),
                NextActionDue = command.NextActionDue,
            };

            _interactionRepository.Add(interaction);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(interaction.Id);
        }
    }
}
