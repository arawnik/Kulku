using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover;
using Kulku.Application.Cover.Models;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

partial class Index(
    IQueryHandler<GetIntroduction.Query, IntroductionModel?> introHandler,
    IQueryHandler<GetKeywords.Query, IReadOnlyList<KeywordModel>> keywordsHandler,
    IQueryHandler<GetExperiences.Query, IReadOnlyList<ExperienceModel>> experienceHandler,
    IQueryHandler<GetEducations.Query, IReadOnlyList<EducationModel>> educationHandler,
    IQueryHandler<GetProjects.Query, IReadOnlyList<ProjectModel>> projectsHandler,
    ILanguageContext languageContext
)
{
    private readonly IQueryHandler<GetIntroduction.Query, IntroductionModel?> _introHandler =
        introHandler;
    private readonly IQueryHandler<
        GetKeywords.Query,
        IReadOnlyList<KeywordModel>
    > _keywordsHandler = keywordsHandler;
    private readonly IQueryHandler<
        GetExperiences.Query,
        IReadOnlyList<ExperienceModel>
    > _experienceHandler = experienceHandler;
    private readonly IQueryHandler<
        GetEducations.Query,
        IReadOnlyList<EducationModel>
    > _educationHandler = educationHandler;
    private readonly IQueryHandler<
        GetProjects.Query,
        IReadOnlyList<ProjectModel>
    > _projectsHandler = projectsHandler;
    private readonly ILanguageContext _languageContext = languageContext;

    private IntroductionModel? IntroductionPreview { get; set; }
    private IReadOnlyList<KeywordModel> LanguageKeywords { get; set; } = [];
    private IReadOnlyList<KeywordModel> SkillKeywords { get; set; } = [];
    private IReadOnlyList<KeywordModel> TechnologyKeywords { get; set; } = [];
    private IReadOnlyList<ExperienceModel> Experiences { get; set; } = [];
    private IReadOnlyList<EducationModel> Educations { get; set; } = [];
    private IReadOnlyList<ProjectModel> Projects { get; set; } = [];

    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        var lang = _languageContext.Current;

        var ires = await _introHandler.Handle(new GetIntroduction.Query(lang), CancellationToken);
        if (ires.IsSuccess)
        {
            IntroductionPreview = ires.Value;
        }

        var lres = await _keywordsHandler.Handle(
            new GetKeywords.Query(Domain.Projects.KeywordType.Language, lang),
            CancellationToken
        );
        if (lres.IsSuccess)
            LanguageKeywords = lres.Value ?? [];

        var sres = await _keywordsHandler.Handle(
            new GetKeywords.Query(Domain.Projects.KeywordType.Skill, lang),
            CancellationToken
        );
        if (sres.IsSuccess)
            SkillKeywords = sres.Value ?? [];

        var tres = await _keywordsHandler.Handle(
            new GetKeywords.Query(Domain.Projects.KeywordType.Technology, lang),
            CancellationToken
        );
        if (tres.IsSuccess)
            TechnologyKeywords = tres.Value ?? [];

        var eres = await _experienceHandler.Handle(
            new GetExperiences.Query(lang),
            CancellationToken
        );
        if (eres.IsSuccess)
            Experiences = eres.Value ?? [];

        var edres = await _educationHandler.Handle(
            new GetEducations.Query(lang),
            CancellationToken
        );
        if (edres.IsSuccess)
            Educations = edres.Value ?? [];

        var pres = await _projectsHandler.Handle(new GetProjects.Query(lang), CancellationToken);
        if (pres.IsSuccess)
            Projects = pres.Value ?? [];

        _loaded = true;
    }
}
