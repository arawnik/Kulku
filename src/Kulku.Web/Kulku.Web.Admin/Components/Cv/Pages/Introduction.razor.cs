using Kulku.Application.Cover.Introduction;
using Kulku.Application.Cover.Introduction.Models;
using Kulku.Web.Admin.Components.Cv.Components;
using Kulku.Web.Admin.Components.Shared;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

#pragma warning disable CA1724 // Type name conflicts with imported namespace (Blazor page name)
partial class Introduction(
    IQueryHandler<
        GetIntroductionTranslations.Query,
        IReadOnlyList<IntroductionTranslationsModel>
    > translationsHandler,
    IQueryHandler<GetIntroductionDetail.Query, IntroductionTranslationsModel?> detailHandler,
    ICommandHandler<UpdateIntroduction.Command> updateHandler,
    ICommandHandler<CreateIntroduction.Command, Guid> createHandler,
    ICommandHandler<DeleteIntroduction.Command> deleteHandler
)
{
    private List<IntroductionTranslationsModel> _allIntroductions = [];
    private IntroductionTranslationsModel? _active;
    private List<IntroductionTranslationsModel> _scheduled = [];
    private List<IntroductionTranslationsModel> _history = [];
    private bool _loaded;
    private bool _showHistory;
    private ModalMode? _modalMode;
    private IntroductionTranslationsModel? CurrentEditModel { get; set; }
    private bool IsSaving { get; set; }
    private string? _errorMessage;
    private IntroductionEditModal? _editModal;

    protected override async Task OnInitializedAsync()
    {
        await LoadIntroductionsAsync();
    }

    private async Task LoadIntroductionsAsync()
    {
        var result = await translationsHandler.Handle(
            new GetIntroductionTranslations.Query(),
            CancellationToken
        );

        _allIntroductions = result.IsSuccess && result.Value is not null ? [.. result.Value] : [];

        CategorizeIntroductions();
        _loaded = true;
    }

    private void CategorizeIntroductions()
    {
        var now = DateTime.UtcNow;
        var published = _allIntroductions.Where(i => i.PubDate <= now).ToList();
        _scheduled = [.. _allIntroductions.Where(i => i.PubDate > now).OrderBy(i => i.PubDate)];

        _active = published.FirstOrDefault(); // Already sorted PubDate desc from query
        _history = [.. published.Skip(1)];
    }

    private async Task HandleCreate()
    {
        _errorMessage = null;

        var blankTranslations = BuildBlankTranslations(lc => new IntroductionTranslationItem(
            lc,
            string.Empty,
            string.Empty,
            string.Empty
        ));

        CurrentEditModel = new IntroductionTranslationsModel(
            IntroductionId: Guid.NewGuid(),
            AvatarUrl: string.Empty,
            SmallAvatarUrl: string.Empty,
            PubDate: DateTime.UtcNow,
            Translations: blankTranslations
        );
        _modalMode = ModalMode.Create;
    }

    private async Task HandleEdit(Guid introductionId)
    {
        _errorMessage = null;

        var result = await detailHandler.Handle(
            new GetIntroductionDetail.Query(introductionId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            CurrentEditModel = result.Value;
            _modalMode = ModalMode.Edit;
        }
        else
        {
            _errorMessage = "Failed to load introduction details.";
        }
    }

    private async Task HandleSave(IntroductionTranslationsModel model)
    {
        _errorMessage = null;
        IsSaving = true;

        try
        {
            var translations = model
                .Translations.Select(t => new IntroductionTranslationDto(
                    t.Language,
                    t.Title,
                    t.Tagline,
                    t.Content
                ))
                .ToList();

            if (_modalMode == ModalMode.Create)
            {
                var createResult = await createHandler.Handle(
                    new CreateIntroduction.Command(
                        model.AvatarUrl,
                        model.SmallAvatarUrl,
                        model.PubDate,
                        translations
                    ),
                    CancellationToken
                );

                if (
                    !TryHandleError(
                        createResult,
                        e => _editModal?.SetServerErrors(e),
                        ref _errorMessage,
                        "Failed to create introduction. Please try again."
                    )
                )
                {
                    CloseEditor();
                    await LoadIntroductionsAsync();
                }

                return;
            }

            var result = await updateHandler.Handle(
                new UpdateIntroduction.Command(
                    model.IntroductionId,
                    model.AvatarUrl,
                    model.SmallAvatarUrl,
                    model.PubDate,
                    translations
                ),
                CancellationToken
            );

            if (
                !TryHandleError(
                    result,
                    e => _editModal?.SetServerErrors(e),
                    ref _errorMessage,
                    "Failed to save changes. Please try again."
                )
            )
            {
                CloseEditor();
                await LoadIntroductionsAsync();
            }
        }
        finally
        {
            IsSaving = false;
        }
    }

    private async Task HandleDelete(Guid introductionId)
    {
        _errorMessage = null;

        var result = await deleteHandler.Handle(
            new DeleteIntroduction.Command(introductionId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadIntroductionsAsync();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? "Failed to delete introduction.";
        }
    }

    private void HandleCancel()
    {
        CloseEditor();
    }

    private void CloseEditor()
    {
        _modalMode = null;
        CurrentEditModel = null;
        _errorMessage = null;
    }
}
