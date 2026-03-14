using Kulku.Application.Cover.Education;
using Kulku.Application.Cover.Education.Models;
using Kulku.Web.Admin.Components.Cv.Components;
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
    ICommandHandler<UpdateEducation.Command> updateHandler
)
{
    private IReadOnlyList<EducationTranslationsModel> Educations { get; set; } = [];
    private bool _loaded;
    private Guid? EditingEducationId { get; set; }
    private EducationTranslationsModel? CurrentEditModel { get; set; }
    private bool IsSaving { get; set; }
    private string? _errorMessage;
    private EducationEditModal? _editModal;

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
                        .ThenByDescending(m => m.EndDate),
                ]
                : [];

        _loaded = true;
    }

    private async Task HandleEdit(Guid educationId)
    {
        _errorMessage = null;
        EditingEducationId = educationId;

        var result = await detailHandler.Handle(
            new GetEducationDetail.Query(educationId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            CurrentEditModel = result.Value;
        }
        else
        {
            EditingEducationId = null;
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

            var result = await updateHandler.Handle(
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

    private void HandleCancel()
    {
        CloseEditor();
    }

    private void CloseEditor()
    {
        EditingEducationId = null;
        CurrentEditModel = null;
        _errorMessage = null;
    }
}
