using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Contact;

/// <summary>
/// Moves a network contact to a different company (or removes affiliation).
/// </summary>
public static class MoveNetworkContact
{
    public sealed record Command(Guid ContactId, Guid? NewCompanyId) : ICommand;

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

            contact.CompanyId = command.NewCompanyId;

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
