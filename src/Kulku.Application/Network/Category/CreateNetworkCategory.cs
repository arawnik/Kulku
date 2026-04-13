using Kulku.Domain.Network;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Network.Category;

/// <summary>
/// Creates a new network category.
/// </summary>
public static class CreateNetworkCategory
{
    public sealed record Command(string Name, string? ColorToken) : ICommand<Guid>;

    internal sealed class Handler(
        INetworkCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, Guid>
    {
        private readonly INetworkCategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = NetworkCategoryCommandValidator.Validate(command.Name);
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var category = new NetworkCategory
            {
                Name = command.Name.Trim(),
                ColorToken = command.ColorToken?.Trim(),
            };

            _categoryRepository.Add(category);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(category.Id);
        }
    }
}
