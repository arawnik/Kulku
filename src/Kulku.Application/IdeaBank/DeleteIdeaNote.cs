using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Deletes a note from an idea.
/// </summary>
public static class DeleteIdeaNote
{
    public sealed record Command(Guid IdeaId, Guid NoteId) : ICommand;

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

            var note = idea.Notes.FirstOrDefault(n => n.Id == command.NoteId);
            if (note is null)
                return Error.NotFound(Strings.NotFound_Note);

            idea.Notes.Remove(note);
            idea.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
