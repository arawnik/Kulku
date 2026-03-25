using Kulku.Application.Cover.Company;
using Kulku.Application.Cover.Company.Models;
using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using Kulku.Web.Admin.Components.Cv.Components;
using Kulku.Web.Admin.Components.Shared;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

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
    ICommandHandler<DeleteCompany.Command> deleteCompanyHandler
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

        var blankTranslations = Defaults
            .SupportedCultures.Select(LanguageCodeFromCulture)
            .Where(lc => lc.HasValue)
            .Select(lc => new ExperienceTranslationItem(lc!.Value, string.Empty, string.Empty))
            .ToList();

        CurrentEditModel = new ExperienceTranslationsModel(
            ExperienceId: Guid.NewGuid(),
            CompanyId: Guid.Empty,
            StartDate: DateOnly.FromDateTime(DateTime.Today),
            EndDate: null,
            Translations: blankTranslations,
            CompanyTranslations: [],
            KeywordNames: []
        );
        _modalMode = ModalMode.Create;
    }

    private async Task HandleEdit(Guid experienceId)
    {
        _errorMessage = null;

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
            _errorMessage = "Failed to load experience details.";
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

            Result result;
            if (_modalMode == ModalMode.Create)
            {
                var createResult = await createHandler.Handle(
                    new CreateExperience.Command(
                        model.CompanyId,
                        model.StartDate,
                        model.EndDate,
                        translations
                    ),
                    CancellationToken
                );

                if (createResult.IsSuccess)
                {
                    CloseEditor();
                    await LoadAllAsync();
                    return;
                }

                if (createResult is IValidationResult createValidation)
                {
                    _editModal?.SetServerErrors(createValidation.Errors);
                    return;
                }

                _errorMessage =
                    createResult.Error?.Message ?? "Failed to create experience. Please try again.";
                return;
            }

            result = await updateHandler.Handle(
                new UpdateExperience.Command(
                    model.ExperienceId,
                    model.StartDate,
                    model.EndDate,
                    translations
                ),
                CancellationToken
            );

            if (result.IsSuccess)
            {
                CloseEditor();
                await LoadAllAsync();
            }
            else if (result is IValidationResult validation)
            {
                _editModal?.SetServerErrors(validation.Errors);
            }
            else
            {
                _errorMessage =
                    result.Error?.Message ?? "Failed to save changes. Please try again.";
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
            _errorMessage = result.Error?.Message ?? "Failed to delete experience entry.";
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

    // ── Company CRUD ─────────────────────────────────

    private void HandleCreateCompany()
    {
        _companyErrorMessage = null;
        _companiesExpanded = true;

        var blankTranslations = Defaults
            .SupportedCultures.Select(LanguageCodeFromCulture)
            .Where(lc => lc.HasValue)
            .Select(lc => new CompanyTranslationItem(lc!.Value, string.Empty, string.Empty))
            .ToList();

        _currentCompany = new CompanyTranslationsModel(
            CompanyId: Guid.NewGuid(),
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
            _errorMessage = "Failed to load company details.";
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
                    new CreateCompany.Command(translations),
                    CancellationToken
                );

                if (result.IsSuccess)
                {
                    CloseCompanyEditor();
                    await LoadAllAsync();
                    return;
                }

                if (result is IValidationResult v)
                {
                    _companyModal?.SetServerErrors(v.Errors);
                    return;
                }

                _companyErrorMessage = result.Error?.Message ?? "Failed to create company.";
                return;
            }

            var updateResult = await updateCompanyHandler.Handle(
                new UpdateCompany.Command(model.CompanyId, translations),
                CancellationToken
            );

            if (updateResult.IsSuccess)
            {
                CloseCompanyEditor();
                await LoadAllAsync();
            }
            else if (updateResult is IValidationResult validation)
            {
                _companyModal?.SetServerErrors(validation.Errors);
            }
            else
            {
                _companyErrorMessage = updateResult.Error?.Message ?? "Failed to save company.";
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
            _errorMessage = result.Error?.Message ?? "Failed to delete company.";
        }
    }

    private void CloseCompanyEditor()
    {
        _companyModalMode = null;
        _currentCompany = null;
        _companyErrorMessage = null;
    }

    // ── Helpers ──────────────────────────────────────

    private static LanguageCode? LanguageCodeFromCulture(string culture) =>
        culture switch
        {
            "en" => LanguageCode.English,
            "fi" => LanguageCode.Finnish,
            _ => null,
        };
}
