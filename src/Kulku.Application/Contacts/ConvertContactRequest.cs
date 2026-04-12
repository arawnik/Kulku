using Kulku.Domain.Contacts;
using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Contacts;

/// <summary>
/// Transactionally converts a contact request into a network interaction.
/// Creates (or reuses) a <see cref="NetworkContact"/>, logs an inbound
/// <see cref="NetworkInteraction"/> via the CV contact form channel,
/// and marks the <see cref="ContactRequest"/> as converted.
/// </summary>
public static class ConvertContactRequest
{
    public sealed record Command(Guid ContactRequestId, Guid CompanyId, string? Summary) : ICommand;

    internal sealed class Handler(
        IContactRequestRepository contactRequestRepository,
        INetworkContactRepository networkContactRepository,
        INetworkInteractionRepository networkInteractionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IContactRequestRepository _contactRequestRepository =
            contactRequestRepository;
        private readonly INetworkContactRepository _networkContactRepository =
            networkContactRepository;
        private readonly INetworkInteractionRepository _networkInteractionRepository =
            networkInteractionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var request = await _contactRequestRepository.GetByIdAsync(
                command.ContactRequestId,
                cancellationToken
            );

            if (request is null)
                return Error.NotFound("Contact request not found.");

            if (request.Status == ContactRequestStatus.Converted)
                return Error.BusinessRule("Contact request has already been converted.");

            // Find or create network contact by email
            var contact = await _networkContactRepository.FindByEmailAsync(
                request.Email,
                cancellationToken
            );

            if (contact is null)
            {
                contact = new NetworkContact
                {
                    CompanyId = command.CompanyId,
                    PersonName = request.Name,
                    Email = request.Email,
                };
                _networkContactRepository.Add(contact);
            }

            // Build the summary from the contact request if not overridden
            var summary = !string.IsNullOrWhiteSpace(command.Summary)
                ? command.Summary.Trim()
                : $"{request.Subject}: {request.Message}";

            var interaction = new NetworkInteraction
            {
                CompanyId = command.CompanyId,
                ContactId = contact.Id,
                Date = request.Timestamp,
                Direction = InteractionDirection.Inbound,
                Channel = InteractionChannel.CvContactForm,
                IsWarmIntro = false,
                Summary = summary,
            };
            _networkInteractionRepository.Add(interaction);

            request.Status = ContactRequestStatus.Converted;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
