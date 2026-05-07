using Kulku.Application.Contacts;
using Kulku.Application.Contacts.Models;
using Microsoft.Extensions.DependencyInjection;
using SoulNETLib.Clean.Application.DependencyInjection;

namespace Kulku.Application;

public static partial class ApplicationDependencyInjection
{
    private static void AddContactServices(this IServiceCollection services)
    {
        // Validators
        services.AddCommandValidator<
            SubmitContactRequest.Command,
            SubmitContactRequest.Validator
        >();

        // Query handlers
        services.AddQueryHandler<
            GetContactRequests.Query,
            IReadOnlyList<ContactRequestModel>,
            GetContactRequests.Handler
        >();
        services.AddQueryHandler<
            GetContactRequestDetail.Query,
            ContactRequestModel?,
            GetContactRequestDetail.Handler
        >();
        services.AddQueryHandler<
            GetContactRequestCountByStatus.Query,
            int,
            GetContactRequestCountByStatus.Handler
        >();

        // Command handlers
        services.AddCommandHandler<SubmitContactRequest.Command, SubmitContactRequest.Handler>();
        services.AddCommandHandler<
            UpdateContactRequestStatus.Command,
            UpdateContactRequestStatus.Handler
        >();
        services.AddCommandHandler<PromoteContactRequest.Command, PromoteContactRequest.Handler>();
    }
}
