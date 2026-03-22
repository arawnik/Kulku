using Kulku.Domain.Ideas;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Adds a timestamped note to an existing idea.
/// </summary>
public static class AddIdeaNote
{
    public sealed record Command(Guid IdeaId, string Content) : ICommand<Guid>;

    internal sealed class Handler(IIdeaRepository ideaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Guid>
    {
        private readonly IIdeaRepository _ideaRepository = ideaRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Content))
                return ValidationResult<Guid>.WithErrors(
                    [Error.Validation(nameof(command.Content), "Note content is required.")]
                );

            var idea = await _ideaRepository.GetByIdAsync(command.IdeaId, cancellationToken);
            if (idea is null)
                return Error.NotFound("Idea not found.");

            var note = new IdeaNote
            {
                IdeaId = idea.Id,
                Content = command.Content,
                CreatedAt = DateTime.UtcNow,
            };

            idea.Notes.Add(note);
            idea.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success(note.Id);
        }
    }
}
