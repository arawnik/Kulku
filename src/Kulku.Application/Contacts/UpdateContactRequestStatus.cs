using Kulku.Domain.Contacts;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Contacts;

/// <summary>
/// Updates the status of a contact request (e.g. mark as spam or dismissed).
/// </summary>
public static class UpdateContactRequestStatus
{
    public sealed record Command(Guid Id, ContactRequestStatus Status) : ICommand;

    internal sealed class Handler(IContactRequestRepository repository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IContactRequestRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var request = await _repository.GetByIdAsync(command.Id, cancellationToken);
            if (request is null)
                return Error.NotFound(nameof(ContactRequest), command.Id);

            request.Status = command.Status;
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
