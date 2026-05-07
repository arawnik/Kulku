using Kulku.Application.Cover.Company;
using Kulku.Application.Cover.Education;
using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Institution;
using Kulku.Application.Cover.Introduction;
using Kulku.Application.Cover.Introduction.Models;
using Kulku.Application.Cover.Models;
using Microsoft.Extensions.DependencyInjection;
using SoulNETLib.Clean.Application.DependencyInjection;

namespace Kulku.Application;

public static partial class ApplicationDependencyInjection
{
    private static void AddCoverServices(this IServiceCollection services)
    {
        // Validators
        services.AddCommandValidator<CreateIntroduction.Command, CreateIntroduction.Validator>();
        services.AddCommandValidator<UpdateIntroduction.Command, UpdateIntroduction.Validator>();
        services.AddCommandValidator<CreateExperience.Command, CreateExperience.Validator>();
        services.AddCommandValidator<UpdateExperience.Command, UpdateExperience.Validator>();
        services.AddCommandValidator<CreateEducation.Command, CreateEducation.Validator>();
        services.AddCommandValidator<UpdateEducation.Command, UpdateEducation.Validator>();
        services.AddCommandValidator<CreateCompany.Command, CreateCompany.Validator>();
        services.AddCommandValidator<UpdateCompany.Command, UpdateCompany.Validator>();
        services.AddCommandValidator<CreateInstitution.Command, CreateInstitution.Validator>();
        services.AddCommandValidator<UpdateInstitution.Command, UpdateInstitution.Validator>();

        // Query handlers
        services.AddQueryHandler<
            GetIntroduction.Query,
            IntroductionModel?,
            GetIntroduction.Handler
        >();
        services.AddQueryHandler<
            GetIntroductionTranslations.Query,
            IReadOnlyList<IntroductionTranslationsModel>,
            GetIntroductionTranslations.Handler
        >();
        services.AddQueryHandler<
            GetIntroductionDetail.Query,
            IntroductionTranslationsModel?,
            GetIntroductionDetail.Handler
        >();
        services.AddQueryHandler<
            GetExperiences.Query,
            IReadOnlyList<ExperienceModel>,
            GetExperiences.Handler
        >();
        services.AddQueryHandler<
            GetExperienceTranslations.Query,
            IReadOnlyList<ExperienceTranslationsModel>,
            GetExperienceTranslations.Handler
        >();
        services.AddQueryHandler<
            GetExperienceDetail.Query,
            ExperienceTranslationsModel?,
            GetExperienceDetail.Handler
        >();
        services.AddQueryHandler<
            GetEducations.Query,
            IReadOnlyList<EducationModel>,
            GetEducations.Handler
        >();
        services.AddQueryHandler<
            GetEducationTranslations.Query,
            IReadOnlyList<EducationTranslationsModel>,
            GetEducationTranslations.Handler
        >();
        services.AddQueryHandler<
            GetEducationDetail.Query,
            EducationTranslationsModel?,
            GetEducationDetail.Handler
        >();
        services.AddQueryHandler<
            GetCompanies.Query,
            IReadOnlyList<CompanyTranslationsModel>,
            GetCompanies.Handler
        >();
        services.AddQueryHandler<
            GetCompanyDetail.Query,
            CompanyTranslationsModel?,
            GetCompanyDetail.Handler
        >();
        services.AddQueryHandler<
            GetInstitutions.Query,
            IReadOnlyList<InstitutionTranslationsModel>,
            GetInstitutions.Handler
        >();
        services.AddQueryHandler<
            GetInstitutionDetail.Query,
            InstitutionTranslationsModel?,
            GetInstitutionDetail.Handler
        >();

        // Command handlers
        services.AddCommandHandler<CreateIntroduction.Command, Guid, CreateIntroduction.Handler>();
        services.AddCommandHandler<UpdateIntroduction.Command, UpdateIntroduction.Handler>();
        services.AddCommandHandler<DeleteIntroduction.Command, DeleteIntroduction.Handler>();
        services.AddCommandHandler<CreateExperience.Command, Guid, CreateExperience.Handler>();
        services.AddCommandHandler<UpdateExperience.Command, UpdateExperience.Handler>();
        services.AddCommandHandler<DeleteExperience.Command, DeleteExperience.Handler>();
        services.AddCommandHandler<CreateEducation.Command, Guid, CreateEducation.Handler>();
        services.AddCommandHandler<UpdateEducation.Command, UpdateEducation.Handler>();
        services.AddCommandHandler<DeleteEducation.Command, DeleteEducation.Handler>();
        services.AddCommandHandler<CreateCompany.Command, Guid, CreateCompany.Handler>();
        services.AddCommandHandler<UpdateCompany.Command, UpdateCompany.Handler>();
        services.AddCommandHandler<DeleteCompany.Command, DeleteCompany.Handler>();
        services.AddCommandHandler<CreateInstitution.Command, Guid, CreateInstitution.Handler>();
        services.AddCommandHandler<UpdateInstitution.Command, UpdateInstitution.Handler>();
        services.AddCommandHandler<DeleteInstitution.Command, DeleteInstitution.Handler>();
    }
}
