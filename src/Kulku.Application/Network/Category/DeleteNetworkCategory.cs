using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Category;

/// <summary>
/// Deletes a network category.
/// </summary>
public static class DeleteNetworkCategory
{
    public sealed record Command(Guid CategoryId) : ICommand;

    internal sealed class Handler(
        INetworkCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly INetworkCategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(
                command.CategoryId,
                cancellationToken
            );

            if (category is null)
                return Error.NotFound(Strings.NotFound_Category);

            _categoryRepository.Remove(category);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
