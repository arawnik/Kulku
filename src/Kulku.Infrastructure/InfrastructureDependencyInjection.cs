using System.Diagnostics.CodeAnalysis;
using Kulku.Application.Abstractions.Security;
using Kulku.Application.Cover.Ports;
using Kulku.Application.Projects.Ports;
using Kulku.Domain.Repositories;
using Kulku.Infrastructure.Queries;
using Kulku.Infrastructure.Repositories;
using Kulku.Infrastructure.Security;
using Kulku.Persistence;
using Kulku.Persistence.Data;
using Kulku.Persistence.Pgsql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Infrastructure;

/// <summary>
/// Provides dependency injection configuration for the Infrastructure layer.
///
/// These are typically 3rd party libraries or services that are part of the infrastructure.
/// </summary>
[ExcludeFromCodeCoverage]
public static class InfrastructureDependencyInjection
{
    /// <summary>
    /// Configures and registers infrastructure services in the application's dependency injection container.
    ///
    /// This extension method is used in `Program.cs` to configure the application's infrastructure.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> used to register services.</param>
    /// <param name="configuration">The application configuration for retrieving connection strings and settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with registered infrastructure services.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // SqlServer, Redis, DbContext, EntityFramework, etc.
        services.AddPostgreSQL(configuration);

        services.AddDataProtection().PersistKeysToDbContext<UserDbContext>();

        // Service to service requirements
        services.Configure<RecaptchaOptions>(configuration.GetSection("Recaptcha"));
        services.AddHttpClient<IRecaptchaValidator, RecaptchaValidator>();

        // Providers
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IKeywordRepository, KeywordRepository>();

        services.AddScoped<IIntroductionRepository, IntroductionRepository>();
        services.AddScoped<IExperienceRepository, ExperienceRepository>();
        services.AddScoped<IEducationRepository, EducationRepository>();

        services.AddScoped<IContactRequestRepository, ContactRequestRepository>();

        // Queries
        services.AddScoped<IProjectQueries, ProjectQueries>();
        services.AddScoped<IKeywordQueries, KeywordQueries>();

        services.AddScoped<IIntroductionQueries, IntroductionQueries>();
        services.AddScoped<IExperienceQueries, ExperienceQueries>();
        services.AddScoped<IEducationQueries, EducationQueries>();

        return services;
    }

    /// <summary>
    /// Applies pending database migrations at application startup.
    ///
    /// This method runs EF Core migrations for both the user and application databases.
    /// It is intended to be used **only in development environments** to ensure that
    /// the database schema is always up-to-date without manual intervention.
    ///
    /// **⚠ Warning: Do not use this in production environments** to avoid applying unintended schema changes.
    /// </summary>
    /// <param name="builder">
    /// The application's <see cref="IApplicationBuilder"/> used during startup configuration.
    /// </param>
    /// <returns>
    /// The same <see cref="IApplicationBuilder"/> instance after running migrations.
    /// </returns>
    public static async Task<IApplicationBuilder> RunMigrations(this IApplicationBuilder builder)
    {
        await using var serviceScope = builder.ApplicationServices.CreateAsyncScope();

        var logger = serviceScope
            .ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("MigrationRunner");

        // Resolve database contexts
        await using var userDbContext =
            serviceScope.ServiceProvider.GetRequiredService<UserDbContext>();
        await using var appDbContext =
            serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Apply pending migrations
        try
        {
            logger.LogInformation("Applying migrations...");

            await userDbContext.Database.MigrateAsync();

            AppDbInitializer.Initialize(appDbContext);

            logger.LogInformation("Migrations applied successfully.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred during migrations: {Message}", e.Message);
        }

        return builder;
    }
}
