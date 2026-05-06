using Kulku.Application.Network.Category;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Contact;
using Kulku.Application.Network.Interaction;
using Kulku.Application.Network.Models;
using Microsoft.Extensions.DependencyInjection;
using SoulNETLib.Clean.Application.DependencyInjection;

namespace Kulku.Application;

public static partial class ApplicationDependencyInjection
{
    private static void AddNetworkServices(this IServiceCollection services)
    {
        // Validators
        services.AddCommandValidator<
            CreateNetworkCategory.Command,
            CreateNetworkCategory.Validator
        >();
        services.AddCommandValidator<
            UpdateNetworkCategory.Command,
            UpdateNetworkCategory.Validator
        >();
        services.AddCommandValidator<
            CreateNetworkContact.Command,
            CreateNetworkContact.Validator
        >();
        services.AddCommandValidator<
            UpdateNetworkContact.Command,
            UpdateNetworkContact.Validator
        >();
        services.AddCommandValidator<
            CreateNetworkInteraction.Command,
            CreateNetworkInteraction.Validator
        >();
        services.AddCommandValidator<
            UpdateNetworkInteraction.Command,
            UpdateNetworkInteraction.Validator
        >();

        // Query handlers
        services.AddQueryHandler<
            GetNetworkCategories.Query,
            IReadOnlyList<NetworkCategoryModel>,
            GetNetworkCategories.Handler
        >();
        services.AddQueryHandler<
            GetNetworkCompanies.Query,
            IReadOnlyList<NetworkCompanyModel>,
            GetNetworkCompanies.Handler
        >();
        services.AddQueryHandler<
            GetNetworkCompanyDetail.Query,
            NetworkCompanyDetailModel?,
            GetNetworkCompanyDetail.Handler
        >();
        services.AddQueryHandler<
            GetAvailableNetworkCompanies.Query,
            IReadOnlyList<NetworkAvailableCompanyModel>,
            GetAvailableNetworkCompanies.Handler
        >();
        services.AddQueryHandler<
            GetNetworkContacts.Query,
            IReadOnlyList<NetworkContactModel>,
            GetNetworkContacts.Handler
        >();
        services.AddQueryHandler<
            GetNetworkInteractions.Query,
            IReadOnlyList<NetworkInteractionModel>,
            GetNetworkInteractions.Handler
        >();

        // Command handlers
        services.AddCommandHandler<
            CreateNetworkCategory.Command,
            Guid,
            CreateNetworkCategory.Handler
        >();
        services.AddCommandHandler<UpdateNetworkCategory.Command, UpdateNetworkCategory.Handler>();
        services.AddCommandHandler<DeleteNetworkCategory.Command, DeleteNetworkCategory.Handler>();
        services.AddCommandHandler<
            EnrollNetworkCompany.Command,
            Guid,
            EnrollNetworkCompany.Handler
        >();
        services.AddCommandHandler<UpdateNetworkProfile.Command, UpdateNetworkProfile.Handler>();
        services.AddCommandHandler<
            DisenrollNetworkCompany.Command,
            DisenrollNetworkCompany.Handler
        >();
        services.AddCommandHandler<
            CreateNetworkContact.Command,
            Guid,
            CreateNetworkContact.Handler
        >();
        services.AddCommandHandler<UpdateNetworkContact.Command, UpdateNetworkContact.Handler>();
        services.AddCommandHandler<DeleteNetworkContact.Command, DeleteNetworkContact.Handler>();
        services.AddCommandHandler<MoveNetworkContact.Command, MoveNetworkContact.Handler>();
        services.AddCommandHandler<
            CreateNetworkInteraction.Command,
            Guid,
            CreateNetworkInteraction.Handler
        >();
        services.AddCommandHandler<
            UpdateNetworkInteraction.Command,
            UpdateNetworkInteraction.Handler
        >();
        services.AddCommandHandler<
            DeleteNetworkInteraction.Command,
            DeleteNetworkInteraction.Handler
        >();
    }
}
