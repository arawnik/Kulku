using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Deletes a project and its associated translations and keyword associations.
/// </summary>
public static class DeleteProject
{
    /// <summary>
    /// Command to delete a project.
    /// </summary>
    /// <param name="ProjectId">The project to remove.</param>
    public sealed record Command(Guid ProjectId) : ICommand;

    internal sealed class Handler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(
                command.ProjectId,
                cancellationToken
            );

            if (project is null)
                return Error.NotFound(Strings.NotFound_Project);

            _projectRepository.Remove(project);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
