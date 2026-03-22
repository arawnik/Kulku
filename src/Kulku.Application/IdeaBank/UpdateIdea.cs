using Kulku.Domain.Ideas;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Updates an existing idea's fields and tag associations.
/// </summary>
public static class UpdateIdea
{
    public sealed record Command(
        Guid IdeaId,
        string Title,
        string? Summary,
        string? Description,
        Guid StatusId,
        Guid PriorityId,
        Guid DomainId,
        IReadOnlyList<Guid> TagIds,
        IReadOnlyList<Guid> KeywordIds
    ) : ICommand;

    internal sealed class Handler(IIdeaRepository ideaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IIdeaRepository _ideaRepository = ideaRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = IdeaCommandValidator.Validate(
                command.Title,
                command.DomainId,
                command.StatusId,
                command.PriorityId
            );
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var idea = await _ideaRepository.GetByIdAsync(command.IdeaId, cancellationToken);
            if (idea is null)
                return Error.NotFound("Idea not found.");

            idea.Title = command.Title;
            idea.Summary = command.Summary;
            idea.Description = command.Description;
            idea.StatusId = command.StatusId;
            idea.PriorityId = command.PriorityId;
            idea.DomainId = command.DomainId;
            idea.UpdatedAt = DateTime.UtcNow;

            // Replace tag associations
            idea.IdeaIdeaTags.Clear();
            foreach (var tagId in command.TagIds)
                idea.IdeaIdeaTags.Add(new IdeaIdeaTag { IdeaId = idea.Id, IdeaTagId = tagId });

            // Replace keyword associations
            idea.IdeaKeywords.Clear();
            foreach (var kwId in command.KeywordIds)
                idea.IdeaKeywords.Add(new IdeaKeyword { IdeaId = idea.Id, KeywordId = kwId });

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
