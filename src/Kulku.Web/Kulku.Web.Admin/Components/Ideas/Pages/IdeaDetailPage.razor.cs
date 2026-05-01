using Kulku.Application.Abstractions.Localization;
using Kulku.Application.IdeaBank;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Kulku.Web.Admin.Components.Ideas.Components;
using Kulku.Web.Admin.Resources;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Ideas.Pages;

public partial class IdeaDetailPage(
    IQueryHandler<GetIdeaDetail.Query, IdeaDetailModel?> getIdeaDetailHandler,
    IQueryHandler<GetIdeaDomains.Query, IReadOnlyList<IdeaDomainModel>> getDomainsHandler,
    IQueryHandler<GetIdeaStatuses.Query, IReadOnlyList<IdeaStatusModel>> getStatusesHandler,
    IQueryHandler<GetIdeaPriorities.Query, IReadOnlyList<IdeaPriorityModel>> getPrioritiesHandler,
    IQueryHandler<GetIdeaTags.Query, IReadOnlyList<IdeaTagModel>> getTagsHandler,
    IQueryHandler<GetKeywordsForPicker.Query, IReadOnlyList<KeywordPickerModel>> getKeywordsHandler,
    ICommandHandler<UpdateIdea.Command> updateIdeaHandler,
    ICommandHandler<AddIdeaNote.Command, Guid> addNoteHandler,
    ICommandHandler<DeleteIdeaNote.Command> deleteNoteHandler,
    ILanguageContext languageContext
)
{
    [Microsoft.AspNetCore.Components.Parameter]
    public Guid Id { get; set; }

    private IdeaDetailModel? _idea;
    private IReadOnlyList<IdeaDomainModel> _domains = [];
    private IReadOnlyList<IdeaStatusModel> _statuses = [];
    private IReadOnlyList<IdeaPriorityModel> _priorities = [];
    private IReadOnlyList<IdeaTagModel> _tags = [];
    private IReadOnlyList<KeywordPickerModel> _keywords = [];
    private bool _loaded;

    // ── Edit modal state ──
    private IdeaEditModal? _editModal;
    private IdeaDetailModel? _editingIdea;
    private bool _ideaSaving;
    private string? _ideaError;

    // ── Note add state ──
    private string _newNoteContent = string.Empty;
    private bool _noteSaving;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
        _loaded = true;
    }

    private async Task LoadAsync()
    {
        var language = languageContext.Current;

        var ideaResult = await getIdeaDetailHandler.Handle(
            new GetIdeaDetail.Query(Id, language),
            CancellationToken.None
        );
        _idea = ideaResult.IsSuccess ? ideaResult.Value : null;

        var domainsResult = await getDomainsHandler.Handle(
            new GetIdeaDomains.Query(language),
            CancellationToken.None
        );
        _domains = domainsResult.IsSuccess ? domainsResult.Value ?? [] : [];

        var statusesResult = await getStatusesHandler.Handle(
            new GetIdeaStatuses.Query(language),
            CancellationToken.None
        );
        _statuses = statusesResult.IsSuccess ? statusesResult.Value ?? [] : [];

        var prioritiesResult = await getPrioritiesHandler.Handle(
            new GetIdeaPriorities.Query(language),
            CancellationToken.None
        );
        _priorities = prioritiesResult.IsSuccess ? prioritiesResult.Value ?? [] : [];

        var tagsResult = await getTagsHandler.Handle(
            new GetIdeaTags.Query(),
            CancellationToken.None
        );
        _tags = tagsResult.IsSuccess ? tagsResult.Value ?? [] : [];

        var keywordsResult = await getKeywordsHandler.Handle(
            new GetKeywordsForPicker.Query(),
            CancellationToken.None
        );
        _keywords = keywordsResult.IsSuccess ? keywordsResult.Value ?? [] : [];
    }

    // ── Idea editing ──

    private void HandleEdit()
    {
        _ideaError = null;
        _editingIdea = _idea;
    }

    private async Task HandleSaveIdea(IdeaDetailModel model)
    {
        _ideaSaving = true;
        _ideaError = null;

        try
        {
            var tagIds = model.Tags.Select(t => t.Id).ToList();
            var keywordIds = model.Keywords.Select(k => k.Id).ToList();
            var result = await updateIdeaHandler.Handle(
                new UpdateIdea.Command(
                    model.Id,
                    model.Title,
                    model.Summary,
                    model.Description,
                    model.StatusId,
                    model.PriorityId,
                    model.DomainId,
                    tagIds,
                    keywordIds
                ),
                CancellationToken.None
            );

            if (!result.IsSuccess)
            {
                if (result is IValidationResult validation)
                    _editModal?.SetServerErrors(validation.Errors);
                else
                    _ideaError = Strings.AnErrorOccurred;
                return;
            }

            CloseEditor();
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _ideaError = ex.Message;
        }
        finally
        {
            _ideaSaving = false;
        }
    }

    private void CloseEditor()
    {
        _editingIdea = null;
        _ideaError = null;
    }

    // ── Notes ──

    private async Task HandleAddNote()
    {
        if (string.IsNullOrWhiteSpace(_newNoteContent) || _idea is null)
            return;

        _noteSaving = true;
        try
        {
            var result = await addNoteHandler.Handle(
                new AddIdeaNote.Command(_idea.Id, _newNoteContent),
                CancellationToken.None
            );

            if (result.IsSuccess)
            {
                _newNoteContent = string.Empty;
                await LoadAsync();
            }
        }
        finally
        {
            _noteSaving = false;
        }
    }

    private async Task HandleDeleteNote(Guid noteId)
    {
        if (_idea is null)
            return;

        var result = await deleteNoteHandler.Handle(
            new DeleteIdeaNote.Command(_idea.Id, noteId),
            CancellationToken.None
        );

        if (result.IsSuccess)
            await LoadAsync();
    }
}
