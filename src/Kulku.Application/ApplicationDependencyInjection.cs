using System.Diagnostics.CodeAnalysis;
using Kulku.Application.Contacts;
using Kulku.Application.Cover;
using Kulku.Application.Cover.Models;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Microsoft.Extensions.DependencyInjection;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

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
        services.AddScoped<
            IQueryHandler<GetKeywords.Query, IReadOnlyList<KeywordModel>>,
            GetKeywords.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetProjects.Query, IReadOnlyList<ProjectModel>>,
            GetProjects.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetIntroduction.Query, IntroductionModel?>,
            GetIntroduction.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetExperiences.Query, IReadOnlyList<ExperienceModel>>,
            GetExperiences.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetEducations.Query, IReadOnlyList<EducationModel>>,
            GetEducations.Handler
        >();

        services.AddScoped<
            ICommandHandler<SubmitContactRequest.Command>,
            SubmitContactRequest.Handler
        >();

        return services;
    }
}
