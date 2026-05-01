using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Deletes an idea and all its notes (cascaded).
/// </summary>
public static class DeleteIdea
{
    public sealed record Command(Guid IdeaId) : ICommand;

    internal sealed class Handler(IIdeaRepository ideaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IIdeaRepository _ideaRepository = ideaRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var idea = await _ideaRepository.GetByIdAsync(command.IdeaId, cancellationToken);
            if (idea is null)
                return Error.NotFound(Strings.NotFound_Idea);

            _ideaRepository.Remove(idea);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
