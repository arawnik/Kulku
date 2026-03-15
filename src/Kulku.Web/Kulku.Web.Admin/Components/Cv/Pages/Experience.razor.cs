using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
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
    ICompanyQueries companyQueries
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

    protected override async Task OnInitializedAsync()
    {
        await LoadExperiencesAsync();
    }

    private async Task LoadExperiencesAsync()
    {
        var result = await translationsHandler.Handle(
            new GetExperienceTranslations.Query(),
            CancellationToken
        );

        Experiences =
            result.IsSuccess && result.Value is not null
                ?
                [
                    .. result
                        .Value.OrderBy(m => m.EndDate.HasValue)
                        .ThenByDescending(m => m.EndDate)
                        .ThenByDescending(m => m.StartDate),
                ]
                : [];

        _loaded = true;
    }

    private async Task HandleCreate()
    {
        _errorMessage = null;
        _companies ??= await companyQueries.ListAllWithTranslationsAsync(CancellationToken);

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
                    await LoadExperiencesAsync();
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
                await LoadExperiencesAsync();
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
            await LoadExperiencesAsync();
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

    private static LanguageCode? LanguageCodeFromCulture(string culture) =>
        culture switch
        {
            "en" => LanguageCode.English,
            "fi" => LanguageCode.Finnish,
            _ => null,
        };
}
