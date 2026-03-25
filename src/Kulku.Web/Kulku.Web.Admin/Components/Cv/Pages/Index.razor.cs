using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover.Education;
using Kulku.Application.Cover.Experience;
using Kulku.Application.Cover.Introduction;
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
        var lang = languageContext.Current;

        var ires = await introHandler.Handle(new GetIntroduction.Query(lang), CancellationToken);
        if (ires.IsSuccess)
            IntroductionPreview = ires.Value;

        var lres = await keywordsHandler.Handle(
            new GetKeywords.Query(Domain.Projects.KeywordType.Language, lang),
            CancellationToken
        );
        if (lres.IsSuccess)
            LanguageKeywords = lres.Value ?? [];

        var sres = await keywordsHandler.Handle(
            new GetKeywords.Query(Domain.Projects.KeywordType.Skill, lang),
            CancellationToken
        );
        if (sres.IsSuccess)
            SkillKeywords = sres.Value ?? [];

        var tres = await keywordsHandler.Handle(
            new GetKeywords.Query(Domain.Projects.KeywordType.Technology, lang),
            CancellationToken
        );
        if (tres.IsSuccess)
            TechnologyKeywords = tres.Value ?? [];

        var eres = await experienceHandler.Handle(
            new GetExperiences.Query(lang),
            CancellationToken
        );
        if (eres.IsSuccess)
            Experiences =
            [
                .. (eres.Value ?? [])
                    .OrderBy(m => m.EndDate.HasValue)
                    .ThenByDescending(m => m.EndDate)
                    .ThenByDescending(m => m.StartDate),
            ];

        var edres = await educationHandler.Handle(new GetEducations.Query(lang), CancellationToken);
        if (edres.IsSuccess)
            Educations =
            [
                .. (edres.Value ?? [])
                    .OrderBy(m => m.EndDate.HasValue)
                    .ThenByDescending(m => m.EndDate)
                    .ThenByDescending(m => m.StartDate),
            ];

        var pres = await projectsHandler.Handle(new GetProjects.Query(lang), CancellationToken);
        if (pres.IsSuccess)
            Projects = pres.Value ?? [];

        _loaded = true;
    }

    private static string ResolveProjectImage(string imageUrl) =>
        Uri.TryCreate(imageUrl, UriKind.Absolute, out _)
            ? imageUrl
            : $"/static/projects/{imageUrl}";

    private static string ResolveIntroductionImage(string imageUrl) =>
        Uri.TryCreate(imageUrl, UriKind.Absolute, out _)
            ? imageUrl
            : $"/static/introductions/{imageUrl}";
}
