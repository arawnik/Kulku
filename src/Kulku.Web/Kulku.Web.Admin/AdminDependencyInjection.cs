using System.Diagnostics.CodeAnalysis;
using Kulku.Application.Abstractions.Localization;
using Kulku.Persistence;
using Kulku.Persistence.Data;
using Kulku.Web.Admin.Components.Account;
using Kulku.Web.Admin.Localization;
using Kulku.Web.Admin.Options;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Kulku.Web.Admin;

/// <summary>
/// Dependency injection methods for the Admin project.
/// </summary>
[ExcludeFromCodeCoverage]
public static class AdminDependencyInjection
{
    /// <summary>
    /// Binds admin-specific options from configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register options into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAdminOptions(this IServiceCollection services)
    {
        services
            .AddOptions<ManagementOptions>()
            .BindConfiguration(ManagementOptions.SectionName)
            .ValidateOnStart();

        return services;
    }

    /// <summary>
    /// Registers core admin services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAdminCore(this IServiceCollection services)
    {
        services.AddLocalization();
        services.AddHttpContextAccessor();

        services.AddRazorComponents().AddInteractiveServerComponents();

        services.AddScoped<ILanguageContext, RequestLanguageContext>();

        services.AddScoped<Components.Layout.InboxBadgeNotifier>();

        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        return services;
    }

    /// <summary>
    /// Registers authentication services for the admin interface.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAdminAuthentication(this IServiceCollection services)
    {
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<
            AuthenticationStateProvider,
            IdentityRevalidatingAuthenticationStateProvider
        >();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
            .AddEntityFrameworkStores<UserDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return services;
    }
}
