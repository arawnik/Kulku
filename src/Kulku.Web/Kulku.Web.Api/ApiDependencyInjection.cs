using System.Diagnostics.CodeAnalysis;
using Carter;
using Kulku.Application.Abstractions.Localization;
using Kulku.Web.Api.Localization;
using Microsoft.AspNetCore.HttpOverrides;

namespace Kulku.Web.Api;

/// <summary>
/// Dependency injection methods for the API project.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ApiDependencyInjection
{
    /// <summary>
    /// Registers core API services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApiCore(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor
                | ForwardedHeaders.XForwardedProto
                | ForwardedHeaders.XForwardedHost;

            // Clear defaults so any reverse proxy (NPM, nginx) on a non-loopback network is trusted.
            // Safe as long as the app port is not publicly exposed.
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddLocalization();

        services.AddProblemDetails();
        services.AddCarter();

        services.AddHttpContextAccessor();
        services.AddScoped<ILanguageContext, RequestLanguageContext>();

        return services;
    }

    /// <summary>
    /// Registers CORS policies for the API.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApiCors(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy(
                ApiCorsPolicyNames.Default,
                policy =>
                {
                    // Restrictive policy: only configured origins are allowed.
                    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                }
            );

            options.AddPolicy(
                ApiCorsPolicyNames.AllowAll,
                policy =>
                {
                    // Permissive policy: development only.
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                }
            );
        });

        return services;
    }

    /// <summary>
    /// Registers OpenAPI document generation for the API.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApiOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();

        return services;
    }
}

/// <summary>
/// CORS policy names used by the API project.
/// </summary>
public static class ApiCorsPolicyNames
{
    /// <summary>
    /// Restrictive CORS policy for configured frontend origins.
    /// </summary>
    public const string Default = "DefaultCors";

    /// <summary>
    /// Permissive CORS policy for local development.
    /// </summary>
    public const string AllowAll = "AllowAll";
}
