using Kulku.Application.Cover.Education;
using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Cover.Institution;
using Kulku.Application.Cover.Institution.Models;
using Kulku.Application.Cover.Models;
using Kulku.Web.Admin.Components.Cv.Components;
using Kulku.Web.Admin.Components.Shared;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

#pragma warning disable CA1724 // Type name conflicts with imported namespace (Blazor page name)
partial class Education(
    IQueryHandler<
        GetEducationTranslations.Query,
        IReadOnlyList<EducationTranslationsModel>
    > translationsHandler,
    IQueryHandler<GetEducationDetail.Query, EducationTranslationsModel?> detailHandler,
    ICommandHandler<UpdateEducation.Command> updateHandler,
    ICommandHandler<CreateEducation.Command, Guid> createHandler,
    ICommandHandler<DeleteEducation.Command> deleteHandler,
    IQueryHandler<
        GetInstitutions.Query,
        IReadOnlyList<InstitutionTranslationsModel>
    > institutionsHandler,
    IQueryHandler<
        GetInstitutionDetail.Query,
        InstitutionTranslationsModel?
    > institutionDetailHandler,
    ICommandHandler<CreateInstitution.Command, Guid> createInstitutionHandler,
    ICommandHandler<UpdateInstitution.Command> updateInstitutionHandler,
    ICommandHandler<DeleteInstitution.Command> deleteInstitutionHandler
)
{
    private IReadOnlyList<EducationTranslationsModel> Educations { get; set; } = [];
    private bool _loaded;
    private ModalMode? _modalMode;
    private EducationTranslationsModel? CurrentEditModel { get; set; }
    private bool IsSaving { get; set; }
    private string? _errorMessage;
    private EducationEditModal? _editModal;
    private IReadOnlyList<InstitutionTranslationsModel>? _institutions;

    // Institution inline CRUD state
    private IReadOnlyList<InstitutionTranslationsModel> _allInstitutions = [];
    private bool _institutionsExpanded;
    private ModalMode? _institutionModalMode;
    private InstitutionTranslationsModel? _currentInstitution;
    private InstitutionEditModal? _institutionModal;
    private bool _isSavingInstitution;
    private string? _institutionErrorMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadAllAsync();
    }

    private async Task LoadAllAsync()
    {
        var eduResult = await translationsHandler.Handle(
            new GetEducationTranslations.Query(),
            CancellationToken
        );

        Educations =
            eduResult.IsSuccess && eduResult.Value is not null
                ?
                [
                    .. eduResult
                        .Value.OrderBy(m => m.EndDate.HasValue)
                        .ThenByDescending(m => m.EndDate)
                        .ThenByDescending(m => m.StartDate),
                ]
                : [];

        var instResult = await institutionsHandler.Handle(
            new GetInstitutions.Query(),
            CancellationToken
        );
        _allInstitutions = instResult.IsSuccess ? instResult.Value ?? [] : [];
        _institutions = _allInstitutions;

        _loaded = true;
    }

    // ── Education CRUD ───────────────────────────────

    private void HandleCreate()
    {
        _errorMessage = null;

        var blankTranslations = BuildBlankTranslations(lc => new EducationTranslationItem(
            lc,
            string.Empty,
            string.Empty
        ));

        CurrentEditModel = new EducationTranslationsModel(
            EducationId: Guid.NewGuid(),
            InstitutionId: Guid.Empty,
            StartDate: DateOnly.FromDateTime(DateTime.Today),
            EndDate: null,
            Translations: blankTranslations,
            InstitutionTranslations: []
        );
        _modalMode = ModalMode.Create;
    }

    private async Task HandleEdit(Guid educationId)
    {
        _errorMessage = null;

        var result = await detailHandler.Handle(
            new GetEducationDetail.Query(educationId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            CurrentEditModel = result.Value;
            _modalMode = ModalMode.Edit;
        }
        else
        {
            _errorMessage = "Failed to load education details.";
        }
    }

    private async Task HandleSave(EducationTranslationsModel model)
    {
        _errorMessage = null;
        IsSaving = true;

        try
        {
            var translations = model
                .Translations.Select(t => new EducationTranslationDto(
                    t.Language,
                    t.Title,
                    t.Description
                ))
                .ToList();

            if (_modalMode == ModalMode.Create)
            {
                var createResult = await createHandler.Handle(
                    new CreateEducation.Command(
                        model.InstitutionId,
                        model.StartDate,
                        model.EndDate,
                        translations
                    ),
                    CancellationToken
                );

                if (
                    !TryHandleError(
                        createResult,
                        e => _editModal?.SetServerErrors(e),
                        ref _errorMessage,
                        "Failed to create education. Please try again."
                    )
                )
                {
                    CloseEditor();
                    await LoadAllAsync();
                }

                return;
            }

            var result = await updateHandler.Handle(
                new UpdateEducation.Command(
                    model.EducationId,
                    model.StartDate,
                    model.EndDate,
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
                await LoadAllAsync();
            }
        }
        finally
        {
            IsSaving = false;
        }
    }

    private async Task HandleDelete(Guid educationId)
    {
        _errorMessage = null;

        var result = await deleteHandler.Handle(
            new DeleteEducation.Command(educationId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadAllAsync();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? "Failed to delete education entry.";
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

    // ── Institution CRUD ─────────────────────────────

    private void HandleCreateInstitution()
    {
        _institutionErrorMessage = null;
        _institutionsExpanded = true;

        var blankTranslations = BuildBlankTranslations(lc => new InstitutionTranslationItem(
            lc,
            string.Empty,
            null,
            string.Empty
        ));

        _currentInstitution = new InstitutionTranslationsModel(
            InstitutionId: Guid.NewGuid(),
            EducationCount: 0,
            Translations: blankTranslations
        );

        _institutionModalMode = ModalMode.Create;
    }

    private async Task HandleEditInstitution(Guid id)
    {
        _institutionErrorMessage = null;
        var result = await institutionDetailHandler.Handle(
            new GetInstitutionDetail.Query(id),
            CancellationToken
        );

        if (result.IsSuccess && result.Value is not null)
        {
            _currentInstitution = result.Value;
            _institutionModalMode = ModalMode.Edit;
        }
        else
        {
            _errorMessage = "Failed to load institution details.";
        }
    }

    private async Task HandleSaveInstitution(InstitutionTranslationsModel model)
    {
        _institutionErrorMessage = null;
        _isSavingInstitution = true;

        try
        {
            var translations = model
                .Translations.Select(t => new InstitutionTranslationDto(
                    t.Language,
                    t.Name,
                    t.Department,
                    t.Description
                ))
                .ToList();

            if (_institutionModalMode == ModalMode.Create)
            {
                var result = await createInstitutionHandler.Handle(
                    new CreateInstitution.Command(translations),
                    CancellationToken
                );

                if (
                    !TryHandleError(
                        result,
                        e => _institutionModal?.SetServerErrors(e),
                        ref _institutionErrorMessage,
                        "Failed to create institution."
                    )
                )
                {
                    CloseInstitutionEditor();
                    await LoadAllAsync();
                }

                return;
            }

            var updateResult = await updateInstitutionHandler.Handle(
                new UpdateInstitution.Command(model.InstitutionId, translations),
                CancellationToken
            );

            if (
                !TryHandleError(
                    updateResult,
                    e => _institutionModal?.SetServerErrors(e),
                    ref _institutionErrorMessage,
                    "Failed to save institution."
                )
            )
            {
                CloseInstitutionEditor();
                await LoadAllAsync();
            }
        }
        finally
        {
            _isSavingInstitution = false;
        }
    }

    private async Task HandleDeleteInstitution(Guid id)
    {
        _errorMessage = null;
        var result = await deleteInstitutionHandler.Handle(
            new DeleteInstitution.Command(id),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadAllAsync();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? "Failed to delete institution.";
        }
    }

    private void CloseInstitutionEditor()
    {
        _institutionModalMode = null;
        _currentInstitution = null;
        _institutionErrorMessage = null;
    }
}
