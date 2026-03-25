using Kulku.Application.Projects.Models;
using Kulku.Domain.Projects;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Creates a new project with translations and keyword associations.
/// </summary>
public static class CreateProject
{
    /// <summary>
    /// Command to create a new project.
    /// </summary>
    /// <param name="Url">The project URL.</param>
    /// <param name="ImageUrl">The project image/screenshot URL.</param>
    /// <param name="Order">Display order (lower = higher priority).</param>
    /// <param name="Translations">Localized name, info, and description per language.</param>
    /// <param name="KeywordIds">IDs of keywords to associate with this project.</param>
    public sealed record Command(
        Uri Url,
        string ImageUrl,
        int Order,
        IReadOnlyList<ProjectTranslationDto> Translations,
        IReadOnlyList<Guid> KeywordIds
    ) : ICommand<Guid>;

    internal sealed class Handler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Guid>
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = ProjectCommandValidator.Validate(
                command.Url,
                command.ImageUrl,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var project = new Project
            {
                Url = command.Url,
                ImageUrl = command.ImageUrl,
                Order = command.Order,
                Translations =
                [
                    .. command.Translations.Select(t => new ProjectTranslation
                    {
                        Language = t.Language,
                        Name = t.Name,
                        Info = t.Info,
                        Description = t.Description,
                    }),
                ],
                ProjectKeywords =
                [
                    .. command.KeywordIds.Select(kwId => new ProjectKeyword { KeywordId = kwId }),
                ],
            };

            _projectRepository.Add(project);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(project.Id);
        }
    }
}
