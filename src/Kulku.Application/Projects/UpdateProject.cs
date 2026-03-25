using Kulku.Application.Projects.Models;
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
                return Error.NotFound("Project not found.");

            project.Url = command.Url;
            project.ImageUrl = command.ImageUrl;
            project.Order = command.Order;

            MergeTranslations(project, command.Translations);
            SyncKeywords(project, command.KeywordIds);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static void MergeTranslations(
            Project project,
            IReadOnlyList<ProjectTranslationDto> incoming
        )
        {
            var existing = project.Translations.ToDictionary(t => t.Language);
            project.Translations.Clear();

            foreach (var dto in incoming)
            {
                if (existing.TryGetValue(dto.Language, out var translation))
                {
                    translation.Name = dto.Name;
                    translation.Info = dto.Info;
                    translation.Description = dto.Description;
                    project.Translations.Add(translation);
                }
                else
                {
                    project.Translations.Add(
                        new ProjectTranslation
                        {
                            Language = dto.Language,
                            Name = dto.Name,
                            Info = dto.Info,
                            Description = dto.Description,
                        }
                    );
                }
            }
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
