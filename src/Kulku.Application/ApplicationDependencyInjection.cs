using System.Diagnostics.CodeAnalysis;
using Kulku.Application.Contacts;
using Kulku.Application.Contacts.Models;
using Kulku.Application.Cover.Company;
using Kulku.Application.Cover.Education;
using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Institution;
using Kulku.Application.Cover.Introduction;
using Kulku.Application.Cover.Introduction.Models;
using Kulku.Application.Cover.Models;
using Kulku.Application.IdeaBank;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.Network.Category;
using Kulku.Application.Network.Company;
using Kulku.Application.Network.Contact;
using Kulku.Application.Network.Interaction;
using Kulku.Application.Network.Models;
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
            IQueryHandler<GetKeywordsForPicker.Query, IReadOnlyList<KeywordPickerModel>>,
            GetKeywordsForPicker.Handler
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

        services.AddScoped<
            IQueryHandler<GetContactRequests.Query, IReadOnlyList<ContactRequestModel>>,
            GetContactRequests.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetContactRequestDetail.Query, ContactRequestModel?>,
            GetContactRequestDetail.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetNewContactRequestCount.Query, int>,
            GetNewContactRequestCount.Handler
        >();

        services.AddScoped<
            ICommandHandler<UpdateContactRequestStatus.Command>,
            UpdateContactRequestStatus.Handler
        >();

        services.AddScoped<
            ICommandHandler<ConvertContactRequest.Command>,
            ConvertContactRequest.Handler
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

        // Idea Bank
        services.AddScoped<
            IQueryHandler<GetIdeas.Query, IReadOnlyList<IdeaListModel>>,
            GetIdeas.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetIdeaDetail.Query, IdeaDetailModel?>,
            GetIdeaDetail.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetIdeaDomains.Query, IReadOnlyList<IdeaDomainModel>>,
            GetIdeaDomains.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetIdeaStatuses.Query, IReadOnlyList<IdeaStatusModel>>,
            GetIdeaStatuses.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetIdeaPriorities.Query, IReadOnlyList<IdeaPriorityModel>>,
            GetIdeaPriorities.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetIdeaTags.Query, IReadOnlyList<IdeaTagModel>>,
            GetIdeaTags.Handler
        >();

        services.AddScoped<ICommandHandler<CreateIdea.Command, Guid>, CreateIdea.Handler>();
        services.AddScoped<ICommandHandler<UpdateIdea.Command>, UpdateIdea.Handler>();
        services.AddScoped<ICommandHandler<DeleteIdea.Command>, DeleteIdea.Handler>();
        services.AddScoped<ICommandHandler<AddIdeaNote.Command, Guid>, AddIdeaNote.Handler>();
        services.AddScoped<ICommandHandler<DeleteIdeaNote.Command>, DeleteIdeaNote.Handler>();
        services.AddScoped<ICommandHandler<CreateIdeaTag.Command, Guid>, CreateIdeaTag.Handler>();
        services.AddScoped<ICommandHandler<UpdateIdeaTag.Command>, UpdateIdeaTag.Handler>();
        services.AddScoped<ICommandHandler<DeleteIdeaTag.Command>, DeleteIdeaTag.Handler>();

        // Network
        services.AddScoped<
            IQueryHandler<GetNetworkCategories.Query, IReadOnlyList<NetworkCategoryModel>>,
            GetNetworkCategories.Handler
        >();
        services.AddScoped<
            ICommandHandler<CreateNetworkCategory.Command, Guid>,
            CreateNetworkCategory.Handler
        >();
        services.AddScoped<
            ICommandHandler<UpdateNetworkCategory.Command>,
            UpdateNetworkCategory.Handler
        >();
        services.AddScoped<
            ICommandHandler<DeleteNetworkCategory.Command>,
            DeleteNetworkCategory.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetNetworkCompanies.Query, IReadOnlyList<NetworkCompanyModel>>,
            GetNetworkCompanies.Handler
        >();
        services.AddScoped<
            IQueryHandler<GetNetworkCompanyDetail.Query, NetworkCompanyDetailModel?>,
            GetNetworkCompanyDetail.Handler
        >();
        services.AddScoped<
            IQueryHandler<
                GetAvailableNetworkCompanies.Query,
                IReadOnlyList<NetworkAvailableCompanyModel>
            >,
            GetAvailableNetworkCompanies.Handler
        >();
        services.AddScoped<
            ICommandHandler<EnrollNetworkCompany.Command, Guid>,
            EnrollNetworkCompany.Handler
        >();
        services.AddScoped<
            ICommandHandler<UpdateNetworkProfile.Command>,
            UpdateNetworkProfile.Handler
        >();
        services.AddScoped<
            ICommandHandler<DisenrollNetworkCompany.Command>,
            DisenrollNetworkCompany.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetNetworkContacts.Query, IReadOnlyList<NetworkContactModel>>,
            GetNetworkContacts.Handler
        >();
        services.AddScoped<
            ICommandHandler<CreateNetworkContact.Command, Guid>,
            CreateNetworkContact.Handler
        >();
        services.AddScoped<
            ICommandHandler<UpdateNetworkContact.Command>,
            UpdateNetworkContact.Handler
        >();
        services.AddScoped<
            ICommandHandler<DeleteNetworkContact.Command>,
            DeleteNetworkContact.Handler
        >();
        services.AddScoped<
            ICommandHandler<MoveNetworkContact.Command>,
            MoveNetworkContact.Handler
        >();

        services.AddScoped<
            IQueryHandler<GetNetworkInteractions.Query, IReadOnlyList<NetworkInteractionModel>>,
            GetNetworkInteractions.Handler
        >();
        services.AddScoped<
            ICommandHandler<CreateNetworkInteraction.Command, Guid>,
            CreateNetworkInteraction.Handler
        >();
        services.AddScoped<
            ICommandHandler<UpdateNetworkInteraction.Command>,
            UpdateNetworkInteraction.Handler
        >();
        services.AddScoped<
            ICommandHandler<DeleteNetworkInteraction.Command>,
            DeleteNetworkInteraction.Handler
        >();

        return services;
    }
}
