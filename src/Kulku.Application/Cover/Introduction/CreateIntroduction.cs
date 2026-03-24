using Kulku.Application.Cover.Introduction.Models;
using Kulku.Domain.Cover;
using Kulku.Domain.Repositories;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Cover.Introduction;

/// <summary>
/// Creates a new introduction with translations.
/// </summary>
public static class CreateIntroduction
{
    public sealed record Command(
        string AvatarUrl,
        string SmallAvatarUrl,
        DateTime PubDate,
        IReadOnlyList<IntroductionTranslationDto> Translations
    ) : ICommand<Guid>;

    internal sealed class Handler(
        IIntroductionRepository introductionRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, Guid>
    {
        private readonly IIntroductionRepository _introductionRepository = introductionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var errors = IntroductionCommandValidator.Validate(
                command.AvatarUrl,
                command.SmallAvatarUrl,
                command.Translations
            );
            if (errors.Length > 0)
                return ValidationResult<Guid>.WithErrors(errors);

            var introduction = new Domain.Cover.Introduction
            {
                AvatarUrl = command.AvatarUrl,
                SmallAvatarUrl = command.SmallAvatarUrl,
                PubDate = DateTime.SpecifyKind(command.PubDate, DateTimeKind.Utc),
                Translations =
                [
                    .. command.Translations.Select(t => new IntroductionTranslation
                    {
                        Language = t.Language,
                        Title = t.Title,
                        Tagline = t.Tagline,
                        Content = t.Content,
                    }),
                ],
            };

            _introductionRepository.Add(introduction);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success(introduction.Id);
        }
    }
}
