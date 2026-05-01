using System.Globalization;
using Kulku.Application.Projects.Ports;
using Kulku.Application.Resources;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Projects;

/// <summary>
/// Deletes a proficiency level. Fails if any keywords still reference it.
/// </summary>
public static class DeleteProficiency
{
    public sealed record Command(Guid ProficiencyId) : ICommand;

    internal sealed class Handler(
        IProficiencyRepository proficiencyRepository,
        IKeywordQueries keywordQueries,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IProficiencyRepository _proficiencyRepository = proficiencyRepository;
        private readonly IKeywordQueries _keywordQueries = keywordQueries;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var proficiency = await _proficiencyRepository.GetByIdAsync(
                command.ProficiencyId,
                cancellationToken
            );

            if (proficiency is null)
                return Error.NotFound(Strings.NotFound_ProficiencyLevel);

            // Guard: reject if keywords still reference this proficiency
            var allKeywords = await _keywordQueries.ListAllWithTranslationsAsync(cancellationToken);
            var referencingCount = allKeywords.Count(k => k.ProficiencyId == command.ProficiencyId);

            if (referencingCount > 0)
                return Error.Validation(
                    "proficiencyId",
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Strings.CannotDelete_ProficiencyReferenced,
                        referencingCount
                    )
                );

            _proficiencyRepository.Remove(proficiency);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
