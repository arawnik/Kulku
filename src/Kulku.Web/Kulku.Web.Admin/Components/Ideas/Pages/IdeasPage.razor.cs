using Kulku.Application.Abstractions.Localization;
using Kulku.Application.IdeaBank;
using Kulku.Application.IdeaBank.Models;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Kulku.Web.Admin.Components.Ideas.Components;
using Kulku.Web.Admin.Components.Shared;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Ideas.Pages;

public partial class IdeasPage(
    IQueryHandler<GetIdeas.Query, IReadOnlyList<IdeaListModel>> getIdeasHandler,
    IQueryHandler<GetIdeaDetail.Query, IdeaDetailModel?> getIdeaDetailHandler,
    IQueryHandler<GetIdeaDomains.Query, IReadOnlyList<IdeaDomainModel>> getDomainsHandler,
    IQueryHandler<GetIdeaStatuses.Query, IReadOnlyList<IdeaStatusModel>> getStatusesHandler,
    IQueryHandler<GetIdeaPriorities.Query, IReadOnlyList<IdeaPriorityModel>> getPrioritiesHandler,
    IQueryHandler<GetIdeaTags.Query, IReadOnlyList<IdeaTagModel>> getTagsHandler,
    IQueryHandler<GetKeywordsForPicker.Query, IReadOnlyList<KeywordPickerModel>> getKeywordsHandler,
    ICommandHandler<CreateIdea.Command, Guid> createIdeaHandler,
    ICommandHandler<UpdateIdea.Command> updateIdeaHandler,
    ICommandHandler<DeleteIdea.Command> deleteIdeaHandler,
    ICommandHandler<CreateIdeaTag.Command, Guid> createTagHandler,
    ICommandHandler<UpdateIdeaTag.Command> updateTagHandler,
    ICommandHandler<DeleteIdeaTag.Command> deleteTagHandler,
    ILanguageContext languageContext
)
{
    // ── Data ──
    private IReadOnlyList<IdeaListModel> _ideas = [];
    private IReadOnlyList<IdeaDomainModel> _domains = [];
    private IReadOnlyList<IdeaStatusModel> _statuses = [];
    private IReadOnlyList<IdeaPriorityModel> _priorities = [];
    private IReadOnlyList<IdeaTagModel> _tags = [];
    private IReadOnlyList<KeywordPickerModel> _keywords = [];

    // ── Filters ──
    private string _searchText = string.Empty;
    private Guid _filterDomainId;
    private Guid _filterStatusId;
    private Guid _filterPriorityId;
    private Guid _filterTagId;

    // ── Idea modal state ──
    private IdeaEditModal? _ideaModal;
    private IdeaDetailModel? _editingIdea;
    private ModalMode _ideaMode = ModalMode.Create;
    private bool _ideaSaving;
    private string? _ideaError;

    // ── Tag modal state ──
    private IdeaTagEditModal? _tagModal;
    private IdeaTagModel? _editingTag;
    private ModalMode _tagMode = ModalMode.Create;
    private bool _tagSaving;
    private string? _tagError;

    private bool _tagsExpanded;

    private IReadOnlyList<IdeaListModel> FilteredIdeas
    {
        get
        {
            IEnumerable<IdeaListModel> result = _ideas;

            if (!string.IsNullOrWhiteSpace(_searchText))
                result = result.Where(i =>
                    i.Title.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                    || (
                        i.Summary?.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                        ?? false
                    )
                );

            if (_filterDomainId != Guid.Empty)
                result = result.Where(i => i.DomainId == _filterDomainId);

            if (_filterStatusId != Guid.Empty)
                result = result.Where(i => i.StatusId == _filterStatusId);

            if (_filterPriorityId != Guid.Empty)
                result = result.Where(i => i.PriorityId == _filterPriorityId);

            if (_filterTagId != Guid.Empty)
            {
                var tagName = _tags.FirstOrDefault(t => t.Id == _filterTagId)?.Name;
                if (tagName is not null)
                    result = result.Where(i => i.TagNames.Contains(tagName));
            }

            return result.ToList();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadAllAsync();
    }

    private async Task LoadAllAsync()
    {
        var language = languageContext.Current;

        var ideasResult = await getIdeasHandler.Handle(
            new GetIdeas.Query(language),
            CancellationToken.None
        );
        _ideas = ideasResult.IsSuccess ? ideasResult.Value ?? [] : [];

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

    // ── Idea CRUD ──

    private void HandleCreateIdea()
    {
        _ideaMode = ModalMode.Create;
        _ideaError = null;
        var defaultDomainId = _domains.Count > 0 ? _domains[0].Id : Guid.Empty;
        var defaultStatusId = _statuses.Count > 0 ? _statuses[0].Id : Guid.Empty;
        var defaultPriorityId =
            _priorities.FirstOrDefault(p => p.Order == 2)?.Id
            ?? (_priorities.Count > 0 ? _priorities[0].Id : Guid.Empty);
        _editingIdea = new IdeaDetailModel(
            Guid.Empty,
            string.Empty,
            null,
            null,
            defaultStatusId,
            string.Empty,
            string.Empty,
            defaultPriorityId,
            string.Empty,
            string.Empty,
            defaultDomainId,
            string.Empty,
            string.Empty,
            [],
            [],
            [],
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }

    private async Task HandleEditIdea(IdeaListModel idea)
    {
        _ideaMode = ModalMode.Edit;
        _ideaError = null;

        var result = await getIdeaDetailHandler.Handle(
            new GetIdeaDetail.Query(idea.Id, languageContext.Current),
            CancellationToken.None
        );

        if (result.IsSuccess)
        {
            _editingIdea = result.Value;
        }
        else
        {
            _ideaError = result.Error?.Message ?? "Failed to load idea details.";
        }
    }

    private async Task HandleSaveIdea(IdeaDetailModel model)
    {
        _ideaSaving = true;
        _ideaError = null;

        try
        {
            var tagIds = model.Tags.Select(t => t.Id).ToList();
            var keywordIds = model.Keywords.Select(k => k.Id).ToList();

            if (_ideaMode == ModalMode.Create)
            {
                var result = await createIdeaHandler.Handle(
                    new CreateIdea.Command(
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
                    HandleValidationErrors(result, _ideaModal);
                    return;
                }
            }
            else
            {
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
                    HandleValidationErrors(result, _ideaModal);
                    return;
                }
            }

            CloseIdeaEditor();
            await LoadAllAsync();
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

    private async Task HandleDeleteIdea(Guid id)
    {
        var result = await deleteIdeaHandler.Handle(
            new DeleteIdea.Command(id),
            CancellationToken.None
        );
        if (!result.IsSuccess)
            return;

        await LoadAllAsync();
    }

    private void CloseIdeaEditor()
    {
        _editingIdea = null;
        _ideaError = null;
    }

    // ── Tag CRUD ──

    private void HandleCreateTag()
    {
        _tagMode = ModalMode.Create;
        _tagError = null;
        _editingTag = new IdeaTagModel(Guid.Empty, string.Empty, null, 0);
    }

    private void HandleEditTag(IdeaTagModel tag)
    {
        _tagMode = ModalMode.Edit;
        _tagError = null;
        _editingTag = tag;
    }

    private async Task HandleSaveTag(IdeaTagModel model)
    {
        _tagSaving = true;
        _tagError = null;

        try
        {
            if (_tagMode == ModalMode.Create)
            {
                var result = await createTagHandler.Handle(
                    new CreateIdeaTag.Command(model.Name, model.ColorHex),
                    CancellationToken.None
                );

                if (!result.IsSuccess)
                {
                    HandleValidationErrors(result, _tagModal);
                    return;
                }
            }
            else
            {
                var result = await updateTagHandler.Handle(
                    new UpdateIdeaTag.Command(model.Id, model.Name, model.ColorHex),
                    CancellationToken.None
                );

                if (!result.IsSuccess)
                {
                    HandleValidationErrors(result, _tagModal);
                    return;
                }
            }

            CloseTagEditor();
            await LoadAllAsync();
        }
        catch (Exception ex)
        {
            _tagError = ex.Message;
        }
        finally
        {
            _tagSaving = false;
        }
    }

    private async Task HandleDeleteTag(Guid id)
    {
        var result = await deleteTagHandler.Handle(
            new DeleteIdeaTag.Command(id),
            CancellationToken.None
        );
        if (!result.IsSuccess)
            return;

        await LoadAllAsync();
    }

    private void CloseTagEditor()
    {
        _editingTag = null;
        _tagError = null;
    }

    // ── Shared ──

    private void HandleValidationErrors<T>(Result<T> result, IdeaEditModal? modal)
    {
        if (result is IValidationResult validation)
            modal?.SetServerErrors(validation.Errors);
        else
            _ideaError = "An error occurred while saving.";
    }

    private void HandleValidationErrors(Result result, IdeaEditModal? modal)
    {
        if (result is IValidationResult validation)
            modal?.SetServerErrors(validation.Errors);
        else
            _ideaError = "An error occurred while saving.";
    }

    private void HandleValidationErrors<T>(Result<T> result, IdeaTagEditModal? modal)
    {
        if (result is IValidationResult validation)
            modal?.SetServerErrors(validation.Errors);
        else
            _tagError = "An error occurred while saving.";
    }

    private void HandleValidationErrors(Result result, IdeaTagEditModal? modal)
    {
        if (result is IValidationResult validation)
            modal?.SetServerErrors(validation.Errors);
        else
            _tagError = "An error occurred while saving.";
    }
}
