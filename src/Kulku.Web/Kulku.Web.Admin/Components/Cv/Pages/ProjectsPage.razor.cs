using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Kulku.Web.Admin.Components.Cv.Components;
using Kulku.Web.Admin.Components.Shared;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

partial class ProjectsPage(
    IQueryHandler<
        GetProjectTranslations.Query,
        IReadOnlyList<ProjectTranslationsModel>
    > translationsHandler,
    IQueryHandler<GetProjectDetail.Query, ProjectTranslationsModel?> detailHandler,
    IQueryHandler<GetKeywordsForPicker.Query, IReadOnlyList<KeywordPickerModel>> getKeywordsHandler,
    ICommandHandler<UpdateProject.Command> updateHandler,
    ICommandHandler<CreateProject.Command, Guid> createHandler,
    ICommandHandler<DeleteProject.Command> deleteHandler
)
{
    private IReadOnlyList<ProjectTranslationsModel> Projects { get; set; } = [];
    private bool _loaded;
    private ModalMode? _modalMode;
    private ProjectTranslationsModel? CurrentEditModel { get; set; }
    private bool IsSaving { get; set; }
    private string? _errorMessage;
    private ProjectEditModal? _editModal;
    private IReadOnlyList<KeywordPickerModel>? _keywords;

    protected override async Task OnInitializedAsync()
    {
        await LoadProjectsAsync();
    }

    private async Task LoadProjectsAsync()
    {
        var result = await translationsHandler.Handle(
            new GetProjectTranslations.Query(),
            CancellationToken
        );

        Projects =
            result.IsSuccess && result.Value is not null
                ? [.. result.Value.OrderBy(p => p.Order)]
                : [];

        _loaded = true;
    }

    private async Task HandleCreate()
    {
        _errorMessage = null;
        _keywords ??= await LoadKeywordsAsync();

        var blankTranslations = BuildBlankTranslations(lc => new ProjectTranslationItem(
            lc,
            string.Empty,
            string.Empty,
            string.Empty
        ));

        CurrentEditModel = new ProjectTranslationsModel(
            ProjectId: Guid.NewGuid(),
            Url: new Uri("https://example.com"),
            ImageUrl: string.Empty,
            Order: 1,
            Translations: blankTranslations,
            KeywordIds: []
        );
        _modalMode = ModalMode.Create;
    }

    private async Task HandleEdit(Guid projectId)
    {
        _errorMessage = null;
        _keywords ??= await LoadKeywordsAsync();

        var result = await detailHandler.Handle(
            new GetProjectDetail.Query(projectId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            CurrentEditModel = result.Value;
            _modalMode = ModalMode.Edit;
        }
        else
        {
            _errorMessage = "Failed to load project details.";
        }
    }

    private async Task HandleSave(ProjectTranslationsModel model)
    {
        _errorMessage = null;
        IsSaving = true;

        try
        {
            var translations = model
                .Translations.Select(t => new ProjectTranslationDto(
                    t.Language,
                    t.Name,
                    t.Info,
                    t.Description
                ))
                .ToList();

            if (_modalMode == ModalMode.Create)
            {
                var createResult = await createHandler.Handle(
                    new CreateProject.Command(
                        model.Url,
                        model.ImageUrl,
                        model.Order,
                        translations,
                        model.KeywordIds
                    ),
                    CancellationToken
                );

                if (
                    !TryHandleError(
                        createResult,
                        e => _editModal?.SetServerErrors(e),
                        ref _errorMessage,
                        "Failed to create project. Please try again."
                    )
                )
                {
                    CloseEditor();
                    await LoadProjectsAsync();
                }

                return;
            }

            var result = await updateHandler.Handle(
                new UpdateProject.Command(
                    model.ProjectId,
                    model.Url,
                    model.ImageUrl,
                    model.Order,
                    translations,
                    model.KeywordIds
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
                await LoadProjectsAsync();
            }
        }
        finally
        {
            IsSaving = false;
        }
    }

    private async Task HandleDelete(Guid projectId)
    {
        _errorMessage = null;

        var result = await deleteHandler.Handle(
            new DeleteProject.Command(projectId),
            CancellationToken
        );

        if (result.IsSuccess)
        {
            await LoadProjectsAsync();
        }
        else
        {
            _errorMessage = result.Error?.Message ?? "Failed to delete project.";
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
        var result = await getKeywordsHandler.Handle(
            new GetKeywordsForPicker.Query(),
            CancellationToken
        );

        if (result.IsSuccess)
            return result.Value ?? [];

        _errorMessage = result.Error?.Message ?? "Failed to load keywords.";
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
}
