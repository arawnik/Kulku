using System.Globalization;
using Kulku.Application.IdeaBank.Ports;
using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Deletes an idea tag. Fails if any ideas still reference it.
/// </summary>
public static class DeleteIdeaTag
{
    public sealed record Command(Guid TagId) : ICommand;

    internal sealed class Handler(
        IIdeaTagRepository tagRepository,
        IIdeaTagQueries tagQueries,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IIdeaTagRepository _tagRepository = tagRepository;
        private readonly IIdeaTagQueries _tagQueries = tagQueries;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetByIdAsync(command.TagId, cancellationToken);
            if (tag is null)
                return Error.NotFound(Strings.NotFound_Tag);

            var allTags = await _tagQueries.ListAllAsync(cancellationToken);
            var tagModel = allTags.FirstOrDefault(t => t.Id == command.TagId);

            if (tagModel is not null && tagModel.IdeaCount > 0)
                return Error.Validation(
                    "tagId",
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Strings.CannotDelete_TagReferenced,
                        tagModel.IdeaCount
                    )
                );

            _tagRepository.Remove(tag);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
