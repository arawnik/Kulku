using System.Diagnostics.CodeAnalysis;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kulku.Persistence.Pgsql;

/// <summary>
/// Provides dependency injection configuration for the PostgreSQL specific persistence layer.
/// </summary>
[ExcludeFromCodeCoverage]
public static class PgsqlDependencyInjection
{
    /// <summary>
    /// Configures and registers SqlServer services in the application's dependency injection container.
    ///
    /// This extension method is used in Infrastructure DI to configure persistence.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> used to register services.</param>
    /// <param name="configuration">The application configuration for retrieving connection strings and settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with registered infrastructure services.</returns>
    public static IServiceCollection AddPostgreSQL(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var defaultConnection =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found."
            );
        var userConnection =
            configuration.GetConnectionString("UserConnection")
            ?? throw new InvalidOperationException("Connection string 'UserConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                defaultConnection,
                b => b.MigrationsAssembly(typeof(PgsqlDependencyInjection).Assembly)
            )
        );
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(
                userConnection,
                b => b.MigrationsAssembly(typeof(PgsqlDependencyInjection).Assembly)
            )
        );

        return services;
    }
}
