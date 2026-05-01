using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Introduction;

/// <summary>
/// Deletes an introduction and its cascade-deleted translations.
/// </summary>
public static class DeleteIntroduction
{
    public sealed record Command(Guid IntroductionId) : ICommand;

    internal sealed class Handler(
        IIntroductionRepository introductionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IIntroductionRepository _introductionRepository = introductionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var introduction = await _introductionRepository.GetByIdAsync(
                command.IntroductionId,
                cancellationToken
            );

            if (introduction is null)
                return Error.NotFound(Strings.NotFound_Introduction);

            _introductionRepository.Remove(introduction);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
