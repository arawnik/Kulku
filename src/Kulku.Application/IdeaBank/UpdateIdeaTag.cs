using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Application.Abstractions.Validation;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Updates an existing idea tag.
/// </summary>
public static class UpdateIdeaTag
{
    public sealed record Command(Guid TagId, string Name, string? ColorHex) : ICommand;

    internal sealed class Validator : ICommandValidator<Command>
    {
        public Task<Error[]> ValidateAsync(Command command, CancellationToken cancellationToken) =>
            Task.FromResult(IdeaTagUpsertRules.Validate(command.Name));
    }

    internal sealed class Handler(IIdeaTagRepository tagRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IIdeaTagRepository _tagRepository = tagRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetByIdAsync(command.TagId, cancellationToken);
            if (tag is null)
                return Error.NotFound(Strings.NotFound_Tag);

            tag.Name = command.Name;
            tag.ColorHex = command.ColorHex;

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
