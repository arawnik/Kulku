using Kulku.Application.Cover.Ports;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Institution;

/// <summary>
/// Deletes an institution. Fails if any educations still reference it.
/// </summary>
public static class DeleteInstitution
{
    public sealed record Command(Guid InstitutionId) : ICommand;

    internal sealed class Handler(
        IInstitutionRepository institutionRepository,
        IInstitutionQueries institutionQueries,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IInstitutionRepository _institutionRepository = institutionRepository;
        private readonly IInstitutionQueries _institutionQueries = institutionQueries;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var institution = await _institutionRepository.GetByIdAsync(
                command.InstitutionId,
                cancellationToken
            );

            if (institution is null)
                return Error.NotFound("Institution not found.");

            var detail = await _institutionQueries.FindByIdWithTranslationsAsync(
                command.InstitutionId,
                cancellationToken
            );

            if (detail is not null && detail.EducationCount > 0)
                return Error.Validation(
                    "institutionId",
                    $"Cannot delete this institution — {detail.EducationCount} education(s) still reference it."
                );

            _institutionRepository.Remove(institution);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
