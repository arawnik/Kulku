using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Education;

/// <summary>
/// Deletes an education entry and its associated translations.
/// </summary>
public static class DeleteEducation
{
    /// <summary>
    /// Command to delete an education entry.
    /// </summary>
    /// <param name="EducationId">The education entry to remove.</param>
    public sealed record Command(Guid EducationId) : ICommand;

    internal sealed class Handler(IEducationRepository educationRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command>
    {
        private readonly IEducationRepository _educationRepository = educationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var education = await _educationRepository.GetByIdAsync(
                command.EducationId,
                cancellationToken
            );

            if (education is null)
                return Error.NotFound("Education not found.");

            _educationRepository.Remove(education);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
