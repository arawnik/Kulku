using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Kulku.Domain.Projects;
using Kulku.Web.Admin.Components.Cv.Components;
using Kulku.Web.Admin.Components.Shared;
using Kulku.Web.Admin.Resources;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

partial class Keywords(
    IQueryHandler<
        GetProficiencies.Query,
        IReadOnlyList<ProficiencyTranslationsModel>
    > proficienciesHandler,
    IQueryHandler<
        GetProficiencyDetail.Query,
        ProficiencyTranslationsModel?
    > proficiencyDetailHandler,
    ICommandHandler<CreateProficiency.Command, Guid> createProficiencyHandler,
    ICommandHandler<UpdateProficiency.Command> updateProficiencyHandler,
    ICommandHandler<DeleteProficiency.Command> deleteProficiencyHandler,
    IQueryHandler<
        GetKeywordTranslations.Query,
        IReadOnlyList<KeywordTranslationsModel>
    > keywordsHandler,
    IQueryHandler<GetKeywordDetail.Query, KeywordTranslationsModel?> keywordDetailHandler,
    ICommandHandler<CreateKeyword.Command, Guid> createKeywordHandler,
    ICommandHandler<UpdateKeyword.Command> updateKeywordHandler,
    ICommandHandler<DeleteKeyword.Command> deleteKeywordHandler
)
{
    private IReadOnlyList<ProficiencyTranslationsModel> _proficiencies = [];
    private IReadOnlyList<KeywordTranslationsModel> _keywords = [];
    private bool _loaded;
    private bool _proficienciesExpanded;
    private bool _showVisibleOnly;
    private string? _errorMessage;
    private bool _isSaving;

    // Proficiency modal state
    private ModalMode? _proficiencyModalMode;
    private ProficiencyTranslationsModel? _currentProficiency;
    private ProficiencyEditModal? _proficiencyModal;

    // Keyword modal state
    private ModalMode? _keywordModalMode;
    private KeywordTranslationsModel? _currentKeyword;
    private KeywordEditModal? _keywordModal;

    protected override async Task OnInitializedAsync()
    {
        await LoadAllAsync();
    }

    private async Task LoadAllAsync()
    {
        var profResult = await proficienciesHandler.Handle(
            new GetProficiencies.Query(),
            CancellationToken
        );
        _proficiencies = profResult.IsSuccess ? profResult.Value ?? [] : [];

        var kwResult = await keywordsHandler.Handle(
            new GetKeywordTranslations.Query(),
            CancellationToken
        );
        _keywords = kwResult.IsSuccess ? kwResult.Value ?? [] : [];

        _loaded = true;
    }

    // ── Proficiency CRUD ───────────────────────────────

    private void HandleCreateProficiency()
    {
        _errorMessage = null;
        _proficienciesExpanded = true;

        var blankTranslations = BuildBlankTranslations(lc => new ProficiencyTranslationItem(
            lc,
            string.Empty,
            string.Empty
        ));

        _currentProficiency = new ProficiencyTranslationsModel(
            ProficiencyId: Guid.NewGuid(),
            Scale: 50,
            Order: _proficiencies.Count + 1,
            KeywordCount: 0,
            Translations: blankTranslations
        );

        _proficiencyModalMode = ModalMode.Create;
    }

    private async Task HandleEditProficiency(Guid id)
    {
        _errorMessage = null;
        var result = await proficiencyDetailHandler.Handle(
            new GetProficiencyDetail.Query(id),
            CancellationToken
        );

        if (result.IsSuccess && result.Value is not null)
        {
            _currentProficiency = result.Value;
            _proficiencyModalMode = ModalMode.Edit;
        }
        else
        {
            _errorMessage = CvStrings.Proficiency_FailedToLoad;
        }
    }

    private async Task HandleSaveProficiency(ProficiencyTranslationsModel model)
    {
        _errorMessage = null;
        _isSaving = true;

        try
        {
            var translations = model
                .Translations.Select(t => new ProficiencyTranslationDto(
                    t.Language,
                    t.Name,
                    t.Description
                ))
                .ToList();

            if (_proficiencyModalMode == ModalMode.Create)
            {
                var result = await createProficiencyHandler.Handle(
                    new CreateProficiency.Command(model.Scale, model.Order, translations),
                    CancellationToken
                );

                if (
                    TryHandleResult(
                        result,
                        e => _proficiencyModal?.SetServerErrors(e),
                        ref _errorMessage,
                        CvStrings.Proficiency_FailedToCreate
                    )
                )
                {
                    CloseProficiencyEditor();
                    await LoadAllAsync();
                }

                return;
            }

            var updateResult = await updateProficiencyHandler.Handle(
                new UpdateProficiency.Command(
                    model.ProficiencyId,
                    model.Scale,
                    model.Order,
                    translations
                ),
                CancellationToken
            );

            if (
                TryHandleResult(
                    updateResult,
                    e => _proficiencyModal?.SetServerErrors(e),
                    ref _errorMessage,
                    CvStrings.Proficiency_FailedToSave
                )
            )
            {
                CloseProficiencyEditor();
                await LoadAllAsync();
            }
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task HandleDeleteProficiency(Guid id)
    {
        _errorMessage = null;
        var result = await deleteProficiencyHandler.Handle(
            new DeleteProficiency.Command(id),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadAllAsync();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? CvStrings.Proficiency_FailedToDelete;
        }
    }

    private void CloseProficiencyEditor()
    {
        _proficiencyModalMode = null;
        _currentProficiency = null;
        _errorMessage = null;
    }

    // ── Keyword CRUD ───────────────────────────────────

    private void HandleCreateKeyword(KeywordType type)
    {
        _errorMessage = null;

        var blankTranslations = BuildBlankTranslations(lc => new KeywordTranslationItem(
            lc,
            string.Empty
        ));

        var typeKeywords = _keywords.Where(k => k.Type == type).ToList();

        var firstProf = _proficiencies.Count > 0 ? _proficiencies[0] : null;

        _currentKeyword = new KeywordTranslationsModel(
            KeywordId: Guid.NewGuid(),
            Type: type,
            Order: typeKeywords.Count + 1,
            Display: true,
            ProficiencyId: firstProf?.ProficiencyId ?? Guid.Empty,
            ProficiencyName: firstProf?.Translations is { Count: > 0 } pts
                ? pts[0].Name
                : string.Empty,
            ProficiencyScale: firstProf?.Scale ?? 0,
            Translations: blankTranslations
        );

        _keywordModalMode = ModalMode.Create;
    }

    private async Task HandleEditKeyword(Guid id)
    {
        _errorMessage = null;
        var result = await keywordDetailHandler.Handle(
            new GetKeywordDetail.Query(id),
            CancellationToken
        );

        if (result.IsSuccess && result.Value is not null)
        {
            _currentKeyword = result.Value;
            _keywordModalMode = ModalMode.Edit;
        }
        else
        {
            _errorMessage = CvStrings.Keywords_FailedToLoad;
        }
    }

    private async Task HandleSaveKeyword(KeywordTranslationsModel model)
    {
        _errorMessage = null;
        _isSaving = true;

        try
        {
            var translations = model
                .Translations.Select(t => new KeywordTranslationDto(t.Language, t.Name))
                .ToList();

            if (_keywordModalMode == ModalMode.Create)
            {
                var result = await createKeywordHandler.Handle(
                    new CreateKeyword.Command(
                        model.Type,
                        model.ProficiencyId,
                        model.Order,
                        model.Display,
                        translations
                    ),
                    CancellationToken
                );

                if (
                    TryHandleResult(
                        result,
                        e => _keywordModal?.SetServerErrors(e),
                        ref _errorMessage,
                        CvStrings.Keywords_FailedToCreate
                    )
                )
                {
                    CloseKeywordEditor();
                    await LoadAllAsync();
                }

                return;
            }

            var updateResult = await updateKeywordHandler.Handle(
                new UpdateKeyword.Command(
                    model.KeywordId,
                    model.Type,
                    model.ProficiencyId,
                    model.Order,
                    model.Display,
                    translations
                ),
                CancellationToken
            );

            if (
                TryHandleResult(
                    updateResult,
                    e => _keywordModal?.SetServerErrors(e),
                    ref _errorMessage,
                    CvStrings.Keywords_FailedToSave
                )
            )
            {
                CloseKeywordEditor();
                await LoadAllAsync();
            }
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task HandleDeleteKeyword(Guid id)
    {
        _errorMessage = null;
        var result = await deleteKeywordHandler.Handle(
            new DeleteKeyword.Command(id),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadAllAsync();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? CvStrings.Keywords_FailedToDelete;
        }
    }

    private void CloseKeywordEditor()
    {
        _keywordModalMode = null;
        _currentKeyword = null;
        _errorMessage = null;
    }
}
