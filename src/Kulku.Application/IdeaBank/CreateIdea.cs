using Kulku.Domain.Ideas;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Creates a new idea.
/// </summary>
public static class CreateIdea
{
    public sealed record Command(
        string Title,
        string? Summary,
        string? Description,
        Guid StatusId,
        Guid PriorityId,
        Guid DomainId,
        IReadOnlyList<Guid> TagIds,
        IReadOnlyList<Guid> KeywordIds
    ) : ICommand<Guid>;

    internal sealed class Handler(IIdeaRepository ideaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Guid>
    {
        private readonly IIdeaRepository _ideaRepository = ideaRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = IdeaCommandValidator.Validate(
                command.Title,
                command.DomainId,
                command.StatusId,
                command.PriorityId
            );
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var idea = new Idea
            {
                Title = command.Title,
                Summary = command.Summary,
                Description = command.Description,
                StatusId = command.StatusId,
                PriorityId = command.PriorityId,
                DomainId = command.DomainId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IdeaIdeaTags =
                [
                    .. command.TagIds.Select(tagId => new IdeaIdeaTag { IdeaTagId = tagId }),
                ],
                IdeaKeywords =
                [
                    .. command.KeywordIds.Select(kwId => new IdeaKeyword { KeywordId = kwId }),
                ],
            };

            _ideaRepository.Add(idea);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(idea.Id);
        }
    }
}
