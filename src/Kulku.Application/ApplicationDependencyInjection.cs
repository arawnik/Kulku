using System.Diagnostics.CodeAnalysis;
using Kulku.Application.Contacts;
using Kulku.Application.Cover.Company;
using Kulku.Application.Cover.Education;
using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Institution;
using Kulku.Application.Cover.Introduction;
using Kulku.Application.Cover.Introduction.Models;
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
        // Application services

        // Business logic services
        services.AddScoped<
            IQueryHandler<GetKeywords.Query, IReadOnlyList<KeywordModel>>,
            GetKeywords.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetKeywordTranslations.Query, IReadOnlyList<KeywordTranslationsModel>>,
            GetKeywordTranslations.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetKeywordDetail.Query, KeywordTranslationsModel?>,
            GetKeywordDetail.Handler
        >();

        services.AddScoped<ICommandHandler<CreateKeyword.Command, Guid>, CreateKeyword.Handler>();

        services.AddScoped<ICommandHandler<UpdateKeyword.Command>, UpdateKeyword.Handler>();

        services.AddScoped<ICommandHandler<DeleteKeyword.Command>, DeleteKeyword.Handler>();

        services.AddScoped<
            IQueryHandler<GetProficiencies.Query, IReadOnlyList<ProficiencyTranslationsModel>>,
            GetProficiencies.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetProficiencyDetail.Query, ProficiencyTranslationsModel?>,
            GetProficiencyDetail.Handler
        >();

        services.AddScoped<
            ICommandHandler<CreateProficiency.Command, Guid>,
            CreateProficiency.Handler
        >();

        services.AddScoped<ICommandHandler<UpdateProficiency.Command>, UpdateProficiency.Handler>();

        services.AddScoped<ICommandHandler<DeleteProficiency.Command>, DeleteProficiency.Handler>();

        services.AddScoped<
            IQueryHandler<GetProjects.Query, IReadOnlyList<ProjectModel>>,
            GetProjects.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetProjectTranslations.Query, IReadOnlyList<ProjectTranslationsModel>>,
            GetProjectTranslations.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetProjectDetail.Query, ProjectTranslationsModel?>,
            GetProjectDetail.Handler
        >();

        services.AddScoped<ICommandHandler<UpdateProject.Command>, UpdateProject.Handler>();

        services.AddScoped<ICommandHandler<DeleteProject.Command>, DeleteProject.Handler>();

        services.AddScoped<ICommandHandler<CreateProject.Command, Guid>, CreateProject.Handler>();

        services.AddScoped<
            IQueryHandler<GetIntroduction.Query, IntroductionModel?>,
            GetIntroduction.Handler
        >();

        services.AddScoped<
            IQueryHandler<
                GetIntroductionTranslations.Query,
                IReadOnlyList<IntroductionTranslationsModel>
            >,
            GetIntroductionTranslations.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetIntroductionDetail.Query, IntroductionTranslationsModel?>,
            GetIntroductionDetail.Handler
        >();

        services.AddScoped<
            ICommandHandler<CreateIntroduction.Command, Guid>,
            CreateIntroduction.Handler
        >();

        services.AddScoped<
            ICommandHandler<UpdateIntroduction.Command>,
            UpdateIntroduction.Handler
        >();

        services.AddScoped<
            ICommandHandler<DeleteIntroduction.Command>,
            DeleteIntroduction.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetExperiences.Query, IReadOnlyList<ExperienceModel>>,
            GetExperiences.Handler
        >();

        services.AddScoped<
            IQueryHandler<
                GetExperienceTranslations.Query,
                IReadOnlyList<ExperienceTranslationsModel>
            >,
            GetExperienceTranslations.Handler
        >();

        services.AddScoped<ICommandHandler<UpdateExperience.Command>, UpdateExperience.Handler>();

        services.AddScoped<ICommandHandler<DeleteExperience.Command>, DeleteExperience.Handler>();

        services.AddScoped<
            ICommandHandler<CreateExperience.Command, Guid>,
            CreateExperience.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetExperienceDetail.Query, ExperienceTranslationsModel?>,
            GetExperienceDetail.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetEducationDetail.Query, EducationTranslationsModel?>,
            GetEducationDetail.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetEducations.Query, IReadOnlyList<EducationModel>>,
            GetEducations.Handler
        >();

        services.AddScoped<
            IQueryHandler<
                GetEducationTranslations.Query,
                IReadOnlyList<EducationTranslationsModel>
            >,
            GetEducationTranslations.Handler
        >();

        services.AddScoped<ICommandHandler<UpdateEducation.Command>, UpdateEducation.Handler>();

        services.AddScoped<ICommandHandler<DeleteEducation.Command>, DeleteEducation.Handler>();

        services.AddScoped<
            ICommandHandler<CreateEducation.Command, Guid>,
            CreateEducation.Handler
        >();

        services.AddScoped<
            ICommandHandler<SubmitContactRequest.Command>,
            SubmitContactRequest.Handler
        >();

        // Company CRUD
        services.AddScoped<
            IQueryHandler<GetCompanies.Query, IReadOnlyList<CompanyTranslationsModel>>,
            GetCompanies.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetCompanyDetail.Query, CompanyTranslationsModel?>,
            GetCompanyDetail.Handler
        >();

        services.AddScoped<ICommandHandler<CreateCompany.Command, Guid>, CreateCompany.Handler>();

        services.AddScoped<ICommandHandler<UpdateCompany.Command>, UpdateCompany.Handler>();

        services.AddScoped<ICommandHandler<DeleteCompany.Command>, DeleteCompany.Handler>();

        // Institution CRUD
        services.AddScoped<
            IQueryHandler<GetInstitutions.Query, IReadOnlyList<InstitutionTranslationsModel>>,
            GetInstitutions.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetInstitutionDetail.Query, InstitutionTranslationsModel?>,
            GetInstitutionDetail.Handler
        >();

        services.AddScoped<
            ICommandHandler<CreateInstitution.Command, Guid>,
            CreateInstitution.Handler
        >();

        services.AddScoped<ICommandHandler<UpdateInstitution.Command>, UpdateInstitution.Handler>();

        services.AddScoped<ICommandHandler<DeleteInstitution.Command>, DeleteInstitution.Handler>();

        return services;
    }
}
