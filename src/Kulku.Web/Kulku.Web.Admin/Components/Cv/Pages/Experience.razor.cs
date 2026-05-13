using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover.Company;
using Kulku.Application.Cover.Company.Models;
using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Models;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Kulku.Web.Admin.Components.Cv.Components;
using Kulku.Web.Admin.Components.Shared;
using Kulku.Web.Admin.Resources;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

#pragma warning disable CA1724 // Type name conflicts with imported namespace (Blazor page name)
partial class Experience(
    IQueryHandler<
        GetExperienceTranslations.Query,
        IReadOnlyList<ExperienceTranslationsModel>
    > translationsHandler,
    IQueryHandler<GetExperienceDetail.Query, ExperienceTranslationsModel?> detailHandler,
    ICommandHandler<UpdateExperience.Command> updateHandler,
    ICommandHandler<CreateExperience.Command, Guid> createHandler,
    ICommandHandler<DeleteExperience.Command> deleteHandler,
    IQueryHandler<GetCompanies.Query, IReadOnlyList<CompanyTranslationsModel>> companiesHandler,
    IQueryHandler<GetCompanyDetail.Query, CompanyTranslationsModel?> companyDetailHandler,
    ICommandHandler<CreateCompany.Command, Guid> createCompanyHandler,
    ICommandHandler<UpdateCompany.Command> updateCompanyHandler,
    ICommandHandler<DeleteCompany.Command> deleteCompanyHandler,
    IQueryHandler<GetKeywordsForPicker.Query, IReadOnlyList<KeywordPickerModel>> keywordsHandler,
    ILanguageContext languageContext
)
{
    private IReadOnlyList<ExperienceTranslationsModel> Experiences { get; set; } = [];
    private bool _loaded;
    private ModalMode? _modalMode;
    private ExperienceTranslationsModel? CurrentEditModel { get; set; }
    private bool IsSaving { get; set; }
    private string? _errorMessage;
    private ExperienceEditModal? _editModal;
    private IReadOnlyList<CompanyTranslationsModel>? _companies;
    private IReadOnlyList<KeywordPickerModel>? _keywords;

    // Company inline CRUD state
    private IReadOnlyList<CompanyTranslationsModel> _allCompanies = [];
    private bool _companiesExpanded;
    private ModalMode? _companyModalMode;
    private CompanyTranslationsModel? _currentCompany;
    private CompanyEditModal? _companyModal;
    private bool _isSavingCompany;
    private string? _companyErrorMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadAllAsync();
        _keywords ??= await LoadKeywordsAsync();
    }

    private async Task LoadAllAsync()
    {
        var expResult = await translationsHandler.Handle(
            new GetExperienceTranslations.Query(),
            CancellationToken
        );

        Experiences =
            expResult.IsSuccess && expResult.Value is not null
                ?
                [
                    .. expResult
                        .Value.OrderBy(m => m.EndDate.HasValue)
                        .ThenByDescending(m => m.EndDate)
                        .ThenByDescending(m => m.StartDate),
                ]
                : [];

        var coResult = await companiesHandler.Handle(new GetCompanies.Query(), CancellationToken);
        _allCompanies = coResult.IsSuccess ? coResult.Value ?? [] : [];
        _companies = _allCompanies;

        _loaded = true;
    }

    // ── Experience CRUD ──────────────────────────────

    private async Task HandleCreate()
    {
        _errorMessage = null;
        _keywords ??= await LoadKeywordsAsync();

        var blankTranslations = BuildBlankTranslations(lc => new ExperienceTranslationItem(
            lc,
            string.Empty,
            string.Empty
        ));

        CurrentEditModel = new ExperienceTranslationsModel(
            ExperienceId: Guid.NewGuid(),
            CompanyId: Guid.Empty,
            StartDate: DateOnly.FromDateTime(DateTime.Today),
            EndDate: null,
            Translations: blankTranslations,
            CompanyTranslations: [],
            KeywordIds: []
        );
        _modalMode = ModalMode.Create;
    }

    private async Task HandleEdit(Guid experienceId)
    {
        _errorMessage = null;
        _keywords ??= await LoadKeywordsAsync();

        var result = await detailHandler.Handle(
            new GetExperienceDetail.Query(experienceId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            CurrentEditModel = result.Value;
            _modalMode = ModalMode.Edit;
        }
        else
        {
            _errorMessage = CvStrings.Experience_FailedToLoad;
        }
    }

    private async Task HandleSave(ExperienceTranslationsModel model)
    {
        _errorMessage = null;
        IsSaving = true;

        try
        {
            var translations = model
                .Translations.Select(t => new ExperienceTranslationDto(
                    t.Language,
                    t.Title,
                    t.Description
                ))
                .ToList();

            if (_modalMode == ModalMode.Create)
            {
                var createResult = await createHandler.Handle(
                    new CreateExperience.Command(
                        model.CompanyId,
                        model.StartDate,
                        model.EndDate,
                        translations,
                        model.KeywordIds
                    ),
                    CancellationToken
                );

                if (
                    TryHandleResult(
                        createResult,
                        e => _editModal?.SetServerErrors(e),
                        ref _errorMessage,
                        CvStrings.Experience_FailedToCreate
                    )
                )
                {
                    CloseEditor();
                    await LoadAllAsync();
                }

                return;
            }

            var result = await updateHandler.Handle(
                new UpdateExperience.Command(
                    model.ExperienceId,
                    model.StartDate,
                    model.EndDate,
                    translations,
                    model.KeywordIds
                ),
                CancellationToken
            );

            if (
                TryHandleResult(
                    result,
                    e => _editModal?.SetServerErrors(e),
                    ref _errorMessage,
                    Strings.FailedToSave
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

    private async Task HandleDelete(Guid experienceId)
    {
        _errorMessage = null;

        var result = await deleteHandler.Handle(
            new DeleteExperience.Command(experienceId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadAllAsync();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? CvStrings.Experience_FailedToDelete;
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

    private async Task<IReadOnlyList<KeywordPickerModel>> LoadKeywordsAsync()
    {
        var result = await keywordsHandler.Handle(
            new GetKeywordsForPicker.Query(languageContext.Current),
            CancellationToken
        );

        if (result.IsSuccess)
            return result.Value ?? [];

        _errorMessage = result.Error?.Message;
        return [];
    }

    /// <summary>
    /// Resolves keyword IDs to display names using the loaded keyword picker data.
    /// </summary>
    private IReadOnlyList<string> ResolveKeywordNames(IReadOnlyList<Guid> keywordIds)
    {
        if (_keywords is null || keywordIds.Count == 0)
            return [];

        var lookup = _keywords.ToDictionary(k => k.Id, k => k.Name);
        return keywordIds.Where(id => lookup.ContainsKey(id)).Select(id => lookup[id]).ToList();
    }

    // ── Company CRUD ─────────────────────────────────

    private void HandleCreateCompany()
    {
        _companyErrorMessage = null;
        _companiesExpanded = true;

        var blankTranslations = BuildBlankTranslations(lc => new CompanyTranslationItem(
            lc,
            string.Empty,
            string.Empty
        ));

        _currentCompany = new CompanyTranslationsModel(
            CompanyId: Guid.NewGuid(),
            Website: null,
            Region: null,
            ExperienceCount: 0,
            Translations: blankTranslations
        );

        _companyModalMode = ModalMode.Create;
    }

    private async Task HandleEditCompany(Guid id)
    {
        _companyErrorMessage = null;
        var result = await companyDetailHandler.Handle(
            new GetCompanyDetail.Query(id),
            CancellationToken
        );

        if (result.IsSuccess && result.Value is not null)
        {
            _currentCompany = result.Value;
            _companyModalMode = ModalMode.Edit;
        }
        else
        {
            _errorMessage = CvStrings.Company_FailedToLoad;
        }
    }

    private async Task HandleSaveCompany(CompanyTranslationsModel model)
    {
        _companyErrorMessage = null;
        _isSavingCompany = true;

        try
        {
            var translations = model
                .Translations.Select(t => new CompanyTranslationDto(
                    t.Language,
                    t.Name,
                    t.Description
                ))
                .ToList();

            if (_companyModalMode == ModalMode.Create)
            {
                var result = await createCompanyHandler.Handle(
                    new CreateCompany.Command(model.Website, model.Region, translations),
                    CancellationToken
                );

                if (
                    TryHandleResult(
                        result,
                        e => _companyModal?.SetServerErrors(e),
                        ref _companyErrorMessage,
                        CvStrings.Company_FailedToCreate
                    )
                )
                {
                    CloseCompanyEditor();
                    await LoadAllAsync();
                }

                return;
            }

            var updateResult = await updateCompanyHandler.Handle(
                new UpdateCompany.Command(
                    model.CompanyId,
                    model.Website,
                    model.Region,
                    translations
                ),
                CancellationToken
            );

            if (
                TryHandleResult(
                    updateResult,
                    e => _companyModal?.SetServerErrors(e),
                    ref _companyErrorMessage,
                    CvStrings.Company_FailedToSave
                )
            )
            {
                CloseCompanyEditor();
                await LoadAllAsync();
            }
        }
        finally
        {
            _isSavingCompany = false;
        }
    }

    private async Task HandleDeleteCompany(Guid id)
    {
        _errorMessage = null;
        var result = await deleteCompanyHandler.Handle(
            new DeleteCompany.Command(id),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadAllAsync();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? CvStrings.Company_FailedToDelete;
        }
    }

    private void CloseCompanyEditor()
    {
        _companyModalMode = null;
        _currentCompany = null;
        _companyErrorMessage = null;
    }
}
