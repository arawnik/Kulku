using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Updates an existing idea tag.
/// </summary>
public static class UpdateIdeaTag
{
    public sealed record Command(Guid TagId, string Name, string? ColorHex) : ICommand;

    internal sealed class Handler(IIdeaTagRepository tagRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IIdeaTagRepository _tagRepository = tagRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = IdeaTagCommandValidator.Validate(command.Name);
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

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
