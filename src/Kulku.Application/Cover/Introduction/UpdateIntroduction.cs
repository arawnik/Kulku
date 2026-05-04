using Kulku.Application.Cover.Introduction.Models;
using Kulku.Application.Resources;
using Kulku.Domain.Abstractions;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Introduction;

/// <summary>
/// Updates an existing introduction and merges its translations.
/// </summary>
public static class UpdateIntroduction
{
    public sealed record Command(
        Guid IntroductionId,
        string AvatarUrl,
        string SmallAvatarUrl,
        DateTime PubDate,
        IReadOnlyList<IntroductionTranslationDto> Translations
    ) : ICommand;

    internal sealed class Handler(
        IIntroductionRepository introductionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command>
    {
        private readonly IIntroductionRepository _introductionRepository = introductionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = IntroductionCommandValidator.Validate(
                command.AvatarUrl,
                command.SmallAvatarUrl,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult.WithErrors(errors);

            var introduction = await _introductionRepository.GetByIdAsync(
                command.IntroductionId,
                cancellationToken
            );

            if (introduction is null)
                return Error.NotFound(Strings.NotFound_Introduction);

            introduction.AvatarUrl = command.AvatarUrl;
            introduction.SmallAvatarUrl = command.SmallAvatarUrl;
            introduction.PubDate = DateTime.SpecifyKind(command.PubDate, DateTimeKind.Utc);

            introduction.MergeTranslations(
                command.Translations,
                (dto, t) =>
                {
                    t.Title = dto.Title;
                    t.Tagline = dto.Tagline;
                    t.Content = dto.Content;
                }
            );

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }
    }
}
