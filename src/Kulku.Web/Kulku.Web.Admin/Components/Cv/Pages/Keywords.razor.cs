using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Projects;
using Kulku.Application.Projects.Models;
using Kulku.Domain.Projects;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

partial class Keywords(
    IQueryHandler<GetKeywords.Query, IReadOnlyList<KeywordModel>> handler,
    ILanguageContext languageContext
)
{
    private readonly IQueryHandler<GetKeywords.Query, IReadOnlyList<KeywordModel>> _handler =
        handler;
    private readonly ILanguageContext _languageContext = languageContext;

    private IReadOnlyList<KeywordModel> LanguageKeywords { get; set; } = [];
    private IReadOnlyList<KeywordModel> SkillKeywords { get; set; } = [];
    private IReadOnlyList<KeywordModel> TechnologyKeywords { get; set; } = [];

    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        var lres = await _handler.Handle(
            new GetKeywords.Query(KeywordType.Language, _languageContext.Current),
            CancellationToken
        );
        if (lres.IsSuccess)
        {
            LanguageKeywords = lres.Value ?? [];
        }

        var sres = await _handler.Handle(
            new GetKeywords.Query(KeywordType.Skill, _languageContext.Current),
            CancellationToken
        );
        if (sres.IsSuccess)
        {
            SkillKeywords = sres.Value ?? [];
        }

        var tres = await _handler.Handle(
            new GetKeywords.Query(KeywordType.Technology, _languageContext.Current),
            CancellationToken
        );
        if (tres.IsSuccess)
        {
            TechnologyKeywords = tres.Value ?? [];
        }

        _loaded = true;
    }
}
