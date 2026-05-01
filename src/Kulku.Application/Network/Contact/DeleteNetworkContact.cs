using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Contact;

/// <summary>
/// Deletes a network contact. Interactions referencing this contact will have
/// their ContactId set to null (via DB cascade).
/// </summary>
public static class DeleteNetworkContact
{
    public sealed record Command(Guid ContactId) : ICommand;

    internal sealed class Handler(
        INetworkContactRepository contactRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly INetworkContactRepository _contactRepository = contactRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(
                command.ContactId,
                cancellationToken
            );

            if (contact is null)
                return Error.NotFound(Strings.NotFound_Contact);

            _contactRepository.Remove(contact);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
