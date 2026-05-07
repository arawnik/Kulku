using Kulku.Application.Abstractions.Security;
using Kulku.Application.Contacts.Models;
using Kulku.Application.Resources;
using Kulku.Domain.Contacts;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Application.Abstractions.Validation;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Contacts;

public static class SubmitContactRequest
{
    public sealed record Command(ContactRequestDto Request) : ICommand;

    internal sealed class Validator : ICommandValidator<Command>
    {
        public Task<Error[]> ValidateAsync(Command command, CancellationToken cancellationToken) =>
            Task.FromResult(
                Validate(
                    command.Request.Name,
                    command.Request.Email,
                    command.Request.Subject,
                    command.Request.Message,
                    command.Request.CaptchaToken
                )
            );
    }

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

    private static Error[] Validate(
        string? name,
        string? email,
        string? subject,
        string? message,
        string? captchaToken
    )
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(Error.Validation(nameof(name), Strings.Validation_ContactNameRequired));

        if (string.IsNullOrWhiteSpace(email))
            errors.Add(Error.Validation(nameof(email), Strings.Validation_ContactEmailRequired));

        if (string.IsNullOrWhiteSpace(subject))
            errors.Add(
                Error.Validation(nameof(subject), Strings.Validation_ContactSubjectRequired)
            );

        if (string.IsNullOrWhiteSpace(message))
            errors.Add(
                Error.Validation(nameof(message), Strings.Validation_ContactMessageRequired)
            );

        if (string.IsNullOrWhiteSpace(captchaToken))
            errors.Add(
                Error.Validation(nameof(captchaToken), Strings.Validation_CaptchaTokenRequired)
            );

        return [.. errors];
    }
}
