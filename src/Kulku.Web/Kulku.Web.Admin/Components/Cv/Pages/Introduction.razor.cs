using Kulku.Application.Abstractions.Localization;
using Kulku.Application.Cover.Introduction;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

#pragma warning disable CA1724 // Type name conflicts with imported namespace (Blazor page name)
partial class Introduction(IQueryHandler<GetIntroduction.Query, IntroductionModel?> handler)
{
    private readonly IQueryHandler<GetIntroduction.Query, IntroductionModel?> _handler = handler;

    private List<IntroductionModel> Introductions { get; set; } = [];
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        foreach (var culture in Defaults.SupportedCultures)
        {
            var lang = LanguageCodeMapper.FromCulture(culture);
            var result = await _handler.Handle(new GetIntroduction.Query(lang), CancellationToken);
            if (result.IsSuccess)
            {
                var introduction = result.Value; // may be null when there is no active introduction
                if (introduction is not null)
                {
                    Introductions.Add(introduction);
                }
            }
        }

        _loaded = true;
    }
}
