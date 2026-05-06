using Kulku.Domain.Ideas;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Application.Abstractions.Validation;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Creates a new idea tag.
/// </summary>
public static class CreateIdeaTag
{
    public sealed record Command(string Name, string? ColorHex) : ICommand<Guid>;

    internal sealed class Validator : ICommandValidator<Command>
    {
        public Task<Error[]> ValidateAsync(Command command, CancellationToken cancellationToken) =>
            Task.FromResult(IdeaTagUpsertRules.Validate(command.Name));
    }

    internal sealed class Handler(IIdeaTagRepository tagRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Guid>
    {
        private readonly IIdeaTagRepository _tagRepository = tagRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var tag = new IdeaTag { Name = command.Name, ColorHex = command.ColorHex };

            _tagRepository.Add(tag);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(tag.Id);
        }
    }
}
