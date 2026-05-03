using Kulku.Application.Abstractions.Security;
using Kulku.Application.Contacts.Models;
using Kulku.Application.Resources;
using Kulku.Domain.Contacts;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Contacts;

public static class SubmitContactRequest
{
    public sealed record Command(ContactRequestDto Request) : ICommand;

    internal sealed class Handler(
        IRecaptchaValidator recaptcha,
        IUnitOfWork unitOfWork,
        IContactRequestRepository repository
    ) : ICommandHandler<Command>
    {
        private readonly IRecaptchaValidator _recaptcha = recaptcha;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IContactRequestRepository _repository = repository;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = ContactRequestCommandValidator.Validate(
                command.Request.Name,
                command.Request.Email,
                command.Request.Subject,
                command.Request.Message,
                command.Request.CaptchaToken
            );
            if (errors.Length > 0)
            {
                return ValidationResult.WithErrors(errors);
            }

            if (!await _recaptcha.ValidateAsync(command.Request.CaptchaToken, cancellationToken))
            {
                return Error.BusinessRule(Strings.BusinessRule_InvalidReCAPTCHA);
            }

            var contactRequest = new ContactRequest
            {
                Name = command.Request.Name,
                Email = command.Request.Email,
                Subject = command.Request.Subject,
                Message = command.Request.Message,
            };
            _repository.Add(contactRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
