using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

partial class ProjectsPage(
    IQueryHandler<GetProjects.Query, IReadOnlyList<ProjectModel>> handler,
    ILanguageContext languageContext
)
{
    private readonly IQueryHandler<GetProjects.Query, IReadOnlyList<ProjectModel>> _handler =
        handler;
    private readonly ILanguageContext _languageContext = languageContext;

    private IReadOnlyList<ProjectModel> ProjectsList { get; set; } = [];
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        var result = await _handler.Handle(
            new GetProjects.Query(_languageContext.Current),
            CancellationToken
        );
        if (result.IsSuccess)
        {
            ProjectsList = result.Value ?? [];
        }

        _loaded = true;
    }
}
