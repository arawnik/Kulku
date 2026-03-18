using Kulku.Domain.Cover;
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
                return Error.NotFound("Introduction not found.");

            introduction.AvatarUrl = command.AvatarUrl;
            introduction.SmallAvatarUrl = command.SmallAvatarUrl;
            introduction.PubDate = DateTime.SpecifyKind(command.PubDate, DateTimeKind.Utc);

            MergeTranslations(introduction, command.Translations);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return Result.Success();
        }

        private static void MergeTranslations(
            Domain.Cover.Introduction introduction,
            IReadOnlyList<IntroductionTranslationDto> incoming
        )
        {
            var existing = introduction.Translations.ToDictionary(t => t.Language);
            introduction.Translations.Clear();

            foreach (var dto in incoming)
            {
                if (existing.TryGetValue(dto.Language, out var translation))
                {
                    translation.Title = dto.Title;
                    translation.Tagline = dto.Tagline;
                    translation.Content = dto.Content;
                    introduction.Translations.Add(translation);
                }
                else
                {
                    introduction.Translations.Add(
                        new IntroductionTranslation
                        {
                            Language = dto.Language,
                            Title = dto.Title,
                            Tagline = dto.Tagline,
                            Content = dto.Content,
                        }
                    );
                }
            }
        }
    }
}
