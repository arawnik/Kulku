using Kulku.Application.IdeaBank;
using Kulku.Application.IdeaBank.Models;
using Microsoft.Extensions.DependencyInjection;
using SoulNETLib.Clean.Application.DependencyInjection;

namespace Kulku.Application;

public static partial class ApplicationDependencyInjection
{
    private static void AddIdeaBankServices(this IServiceCollection services)
    {
        // Validators
        services.AddCommandValidator<CreateIdea.Command, CreateIdea.Validator>();
        services.AddCommandValidator<UpdateIdea.Command, UpdateIdea.Validator>();
        services.AddCommandValidator<AddIdeaNote.Command, AddIdeaNote.Validator>();
        services.AddCommandValidator<CreateIdeaTag.Command, CreateIdeaTag.Validator>();
        services.AddCommandValidator<UpdateIdeaTag.Command, UpdateIdeaTag.Validator>();

        // Query handlers
        services.AddQueryHandler<GetIdeas.Query, IReadOnlyList<IdeaListModel>, GetIdeas.Handler>();
        services.AddQueryHandler<GetIdeaDetail.Query, IdeaDetailModel?, GetIdeaDetail.Handler>();
        services.AddQueryHandler<
            GetIdeaDomains.Query,
            IReadOnlyList<IdeaDomainModel>,
            GetIdeaDomains.Handler
        >();
        services.AddQueryHandler<
            GetIdeaStatuses.Query,
            IReadOnlyList<IdeaStatusModel>,
            GetIdeaStatuses.Handler
        >();
        services.AddQueryHandler<
            GetIdeaPriorities.Query,
            IReadOnlyList<IdeaPriorityModel>,
            GetIdeaPriorities.Handler
        >();
        services.AddQueryHandler<
            GetIdeaTags.Query,
            IReadOnlyList<IdeaTagModel>,
            GetIdeaTags.Handler
        >();

        // Command handlers
        services.AddCommandHandler<CreateIdea.Command, Guid, CreateIdea.Handler>();
        services.AddCommandHandler<UpdateIdea.Command, UpdateIdea.Handler>();
        services.AddCommandHandler<DeleteIdea.Command, DeleteIdea.Handler>();
        services.AddCommandHandler<AddIdeaNote.Command, Guid, AddIdeaNote.Handler>();
        services.AddCommandHandler<DeleteIdeaNote.Command, DeleteIdeaNote.Handler>();
        services.AddCommandHandler<CreateIdeaTag.Command, Guid, CreateIdeaTag.Handler>();
        services.AddCommandHandler<UpdateIdeaTag.Command, UpdateIdeaTag.Handler>();
        services.AddCommandHandler<DeleteIdeaTag.Command, DeleteIdeaTag.Handler>();
    }
}
