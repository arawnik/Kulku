using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Category;

/// <summary>
/// Updates an existing network category.
/// </summary>
public static class UpdateNetworkCategory
{
    public sealed record Command(Guid CategoryId, string Name, string? ColorToken) : ICommand;

    internal sealed class Handler(
        INetworkCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly INetworkCategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = NetworkCategoryCommandValidator.Validate(command.Name);
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var category = await _categoryRepository.GetByIdAsync(
                command.CategoryId,
                cancellationToken
            );

            if (category is null)
                return Error.NotFound("Category not found.");

            category.Name = command.Name.Trim();
            category.ColorToken = command.ColorToken?.Trim();

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
