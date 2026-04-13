using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Contact;

/// <summary>
/// Creates a new network contact, optionally affiliated with a company.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1054:URI-like parameters should not be strings",
    Justification = "Application command DTO; URI validation deferred to presentation edge."
)]
public static class CreateNetworkContact
{
    public sealed record Command(
        Guid? CompanyId,
        string? PersonName,
        string? Email,
        string? Phone,
        string? LinkedInUrl,
        string? Title
    ) : ICommand<Guid>;

    internal sealed class Handler(
        INetworkContactRepository contactRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, Guid>
    {
        private readonly INetworkContactRepository _contactRepository = contactRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = NetworkContactCommandValidator.Validate(
                command.PersonName,
                command.Email,
                command.LinkedInUrl
            );
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var contact = new NetworkContact
            {
                CompanyId = command.CompanyId,
                PersonName = command.PersonName?.Trim(),
                Email = command.Email?.Trim(),
                Phone = command.Phone?.Trim(),
                LinkedInUrl = command.LinkedInUrl?.Trim(),
                Title = command.Title?.Trim(),
            };

            _contactRepository.Add(contact);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(contact.Id);
        }
    }
}
