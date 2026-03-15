using Kulku.Application.Cover.Education;
using Kulku.Application.Cover.Education.Models;
using Kulku.Application.Cover.Models;
using Kulku.Application.Cover.Ports;
using Kulku.Domain;
using Kulku.Web.Admin.Components.Cv.Components;
using Kulku.Web.Admin.Components.Shared;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

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
    IInstitutionQueries institutionQueries
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

    protected override async Task OnInitializedAsync()
    {
        await LoadEducationsAsync();
    }

    private async Task LoadEducationsAsync()
    {
        var result = await translationsHandler.Handle(
            new GetEducationTranslations.Query(),
            CancellationToken
        );

        Educations =
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
        _institutions ??= await institutionQueries.ListAllWithTranslationsAsync(CancellationToken);

        // Build a blank model with translations for all supported languages
        var blankTranslations = Defaults
            .SupportedCultures.Select(LanguageCodeFromCulture)
            .Where(lc => lc.HasValue)
            .Select(lc => new EducationTranslationItem(lc!.Value, string.Empty, string.Empty))
            .ToList();

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

            Result result;
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

                if (createResult.IsSuccess)
                {
                    CloseEditor();
                    await LoadEducationsAsync();
                    return;
                }

                if (createResult is IValidationResult createValidation)
                {
                    _editModal?.SetServerErrors(createValidation.Errors);
                    return;
                }

                _errorMessage =
                    createResult.Error?.Message ?? "Failed to create education. Please try again.";
                return;
            }

            result = await updateHandler.Handle(
                new UpdateEducation.Command(
                    model.EducationId,
                    model.StartDate,
                    model.EndDate,
                    translations
                ),
                CancellationToken
            );

            if (result.IsSuccess)
            {
                CloseEditor();
                await LoadEducationsAsync();
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

    private async Task HandleDelete(Guid educationId)
    {
        _errorMessage = null;

        var result = await deleteHandler.Handle(
            new DeleteEducation.Command(educationId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadEducationsAsync();
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

    private static LanguageCode? LanguageCodeFromCulture(string culture) =>
        culture switch
        {
            "en" => LanguageCode.English,
            "fi" => LanguageCode.Finnish,
            _ => null,
        };
}
