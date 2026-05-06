using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Microsoft.Extensions.DependencyInjection;
using SoulNETLib.Clean.Application.DependencyInjection;

namespace Kulku.Application;

public static partial class ApplicationDependencyInjection
{
    private static void AddProjectServices(this IServiceCollection services)
    {
        // Validators
        services.AddCommandValidator<CreateKeyword.Command, CreateKeyword.Validator>();
        services.AddCommandValidator<UpdateKeyword.Command, UpdateKeyword.Validator>();
        services.AddCommandValidator<CreateProficiency.Command, CreateProficiency.Validator>();
        services.AddCommandValidator<UpdateProficiency.Command, UpdateProficiency.Validator>();
        services.AddCommandValidator<CreateProject.Command, CreateProject.Validator>();
        services.AddCommandValidator<UpdateProject.Command, UpdateProject.Validator>();

        // Query handlers
        services.AddQueryHandler<
            GetKeywords.Query,
            IReadOnlyList<KeywordModel>,
            GetKeywords.Handler
        >();
        services.AddQueryHandler<
            GetKeywordsForPicker.Query,
            IReadOnlyList<KeywordPickerModel>,
            GetKeywordsForPicker.Handler
        >();
        services.AddQueryHandler<
            GetKeywordTranslations.Query,
            IReadOnlyList<KeywordTranslationsModel>,
            GetKeywordTranslations.Handler
        >();
        services.AddQueryHandler<
            GetKeywordDetail.Query,
            KeywordTranslationsModel?,
            GetKeywordDetail.Handler
        >();
        services.AddQueryHandler<
            GetProficiencies.Query,
            IReadOnlyList<ProficiencyTranslationsModel>,
            GetProficiencies.Handler
        >();
        services.AddQueryHandler<
            GetProficiencyDetail.Query,
            ProficiencyTranslationsModel?,
            GetProficiencyDetail.Handler
        >();
        services.AddQueryHandler<
            GetProjects.Query,
            IReadOnlyList<ProjectModel>,
            GetProjects.Handler
        >();
        services.AddQueryHandler<
            GetProjectTranslations.Query,
            IReadOnlyList<ProjectTranslationsModel>,
            GetProjectTranslations.Handler
        >();
        services.AddQueryHandler<
            GetProjectDetail.Query,
            ProjectTranslationsModel?,
            GetProjectDetail.Handler
        >();

        // Command handlers
        services.AddCommandHandler<CreateKeyword.Command, Guid, CreateKeyword.Handler>();
        services.AddCommandHandler<UpdateKeyword.Command, UpdateKeyword.Handler>();
        services.AddCommandHandler<DeleteKeyword.Command, DeleteKeyword.Handler>();
        services.AddCommandHandler<CreateProficiency.Command, Guid, CreateProficiency.Handler>();
        services.AddCommandHandler<UpdateProficiency.Command, UpdateProficiency.Handler>();
        services.AddCommandHandler<DeleteProficiency.Command, DeleteProficiency.Handler>();
        services.AddCommandHandler<CreateProject.Command, Guid, CreateProject.Handler>();
        services.AddCommandHandler<UpdateProject.Command, UpdateProject.Handler>();
        services.AddCommandHandler<DeleteProject.Command, DeleteProject.Handler>();
    }
}
