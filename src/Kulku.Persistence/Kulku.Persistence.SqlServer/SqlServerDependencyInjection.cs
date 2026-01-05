using System.Diagnostics.CodeAnalysis;
using Kulku.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kulku.Persistence.SqlServer;

/// <summary>
/// Provides dependency injection configuration for the SqlServer specific persistence layer.
/// </summary>
[ExcludeFromCodeCoverage]
public static class SqlServerDependencyInjection
{
    /// <summary>
    /// Configures and registers SqlServer services in the application's dependency injection container.
    ///
    /// This extension method is used in Infrastructure DI to configure persistence.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> used to register services.</param>
    /// <param name="configuration">The application configuration for retrieving connection strings and settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with registered infrastructure services.</returns>
    public static IServiceCollection AddSqlServer(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var defaultConnection =
            configuration.GetConnectionString("MsDefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'MsDefaultConnection' not found."
            );
        var userConnection =
            configuration.GetConnectionString("MsUserConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'MsUserConnection' not found."
            );

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                defaultConnection,
                b => b.MigrationsAssembly(typeof(SqlServerDependencyInjection).Assembly)
            )
        );
        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(
                userConnection,
                b => b.MigrationsAssembly(typeof(SqlServerDependencyInjection).Assembly)
            )
        );

        return services;
    }
}
