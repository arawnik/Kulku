using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Kulku.Application;

/// <summary>
/// Provides dependency injection configuration for the Application layer.
///
/// These are typically your own services that are not part of or directly dependent on the infrastructure.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ApplicationDependencyInjection
{
    /// <summary>
    /// Configures and registers application services in the dependency injection container.
    ///
    /// This extension method is used in `Program.cs` to configure and inject application services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> used to register services.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with registered application services.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddLocalization();

        // Application services

        // Business logic services

        return services;
    }
}
