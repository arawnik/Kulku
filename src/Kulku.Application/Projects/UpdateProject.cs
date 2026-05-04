using Kulku.Application.Projects.Models;
using Kulku.Application.Resources;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Updates an existing project, merges its translations, and syncs keyword associations.
/// </summary>
public static class UpdateProject
{
    /// <summary>
    /// Command to update an existing project.
    /// </summary>
    /// <param name="ProjectId">The project to update.</param>
    /// <param name="Url">Updated project URL.</param>
    /// <param name="ImageUrl">Updated image URL.</param>
    /// <param name="Order">Updated display order.</param>
    /// <param name="Translations">Full set of translations to replace existing ones.</param>
    /// <param name="KeywordIds">Full set of keyword IDs to replace existing associations.</param>
    public sealed record Command(
        Guid ProjectId,
        Uri Url,
        string ImageUrl,
        int Order,
        IReadOnlyList<ProjectTranslationDto> Translations,
        IReadOnlyList<Guid> KeywordIds
    ) : ICommand;

    internal sealed class Handler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = ProjectCommandValidator.Validate(
                command.Url,
                command.ImageUrl,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var project = await _projectRepository.GetByIdAsync(
                command.ProjectId,
                cancellationToken
            );

            if (project is null)
                return Error.NotFound(Strings.NotFound_Project);

            project.Url = command.Url;
            project.ImageUrl = command.ImageUrl;
            project.Order = command.Order;

            project.MergeTranslations(
                command.Translations,
                (dto, t) =>
                {
                    t.Name = dto.Name;
                    t.Info = dto.Info;
                    t.Description = dto.Description;
                }
            );

            SyncKeywords(project, command.KeywordIds);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static void SyncKeywords(Project project, IReadOnlyList<Guid> keywordIds)
        {
            project.ProjectKeywords.Clear();

            foreach (var kwId in keywordIds)
            {
                project.ProjectKeywords.Add(new ProjectKeyword { KeywordId = kwId });
            }
        }
    }
}
