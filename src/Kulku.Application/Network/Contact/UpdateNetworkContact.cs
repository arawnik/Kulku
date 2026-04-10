using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Contact;

/// <summary>
/// Updates an existing network contact's details.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1054:URI-like parameters should not be strings",
    Justification = "Application command DTO; URI validation deferred to presentation edge."
)]
public static class UpdateNetworkContact
{
    public sealed record Command(
        Guid ContactId,
        string? PersonName,
        string? Email,
        string? Phone,
        string? LinkedInUrl,
        string? Title
    ) : ICommand;

    internal sealed class Handler(
        INetworkContactRepository contactRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly INetworkContactRepository _contactRepository = contactRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = NetworkContactCommandValidator.Validate(
                command.PersonName,
                command.Email,
                command.LinkedInUrl
            );
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var contact = await _contactRepository.GetByIdAsync(
                command.ContactId,
                cancellationToken
            );

            if (contact is null)
                return Error.NotFound("Contact not found.");

            contact.PersonName = command.PersonName?.Trim();
            contact.Email = command.Email?.Trim();
            contact.Phone = command.Phone?.Trim();
            contact.LinkedInUrl = command.LinkedInUrl?.Trim();
            contact.Title = command.Title?.Trim();

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
