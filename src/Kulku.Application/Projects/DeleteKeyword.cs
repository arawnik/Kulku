using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Deletes a keyword. EF cascades translations and ProjectKeyword associations.
/// </summary>
public static class DeleteKeyword
{
    public sealed record Command(Guid KeywordId) : ICommand;

    internal sealed class Handler(IKeywordRepository keywordRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IKeywordRepository _keywordRepository = keywordRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var keyword = await _keywordRepository.GetByIdAsync(
                command.KeywordId,
                cancellationToken
            );

            if (keyword is null)
                return Error.NotFound(Strings.NotFound_Keyword);

            _keywordRepository.Remove(keyword);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
