using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Experience.Models;
using Kulku.Web.Admin.Components.Cv.Components;
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
    ICommandHandler<UpdateExperience.Command> updateHandler
)
{
    private IReadOnlyList<ExperienceTranslationsModel> Experiences { get; set; } = [];
    private bool _loaded;
    private Guid? EditingExperienceId { get; set; }
    private ExperienceTranslationsModel? CurrentEditModel { get; set; }
    private bool IsSaving { get; set; }
    private string? _errorMessage;
    private ExperienceEditModal? _editModal;

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
                        .ThenByDescending(m => m.EndDate),
                ]
                : [];

        _loaded = true;
    }

    private async Task HandleEdit(Guid experienceId)
    {
        _errorMessage = null;
        EditingExperienceId = experienceId;

        var result = await detailHandler.Handle(
            new GetExperienceDetail.Query(experienceId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            CurrentEditModel = result.Value;
        }
        else
        {
            EditingExperienceId = null;
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

            var result = await updateHandler.Handle(
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

    private void HandleCancel()
    {
        CloseEditor();
    }

    private void CloseEditor()
    {
        EditingExperienceId = null;
        CurrentEditModel = null;
        _errorMessage = null;
    }
}
