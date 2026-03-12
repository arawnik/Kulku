using Kulku.Application.Cover;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

partial class Experience(
    IQueryHandler<
        GetExperienceTranslations.Query,
        IReadOnlyList<ExperienceTranslationsModel>
    > translationsHandler
)
{
    private IReadOnlyList<ExperienceTranslationsModel> Experiences { get; set; } = [];
    private bool _loaded;

    protected static string LanguageDisplayName(LanguageCode code) =>
        code switch
        {
            LanguageCode.English => "English",
            LanguageCode.Finnish => "Suomi",
            _ => code.ToString(),
        };

    protected override async Task OnInitializedAsync()
    {
        var result = await translationsHandler.Handle(
            new GetExperienceTranslations.Query(),
            CancellationToken
        );
        Experiences =
            result.IsSuccess && result.Value is not null
                ? result
                    .Value.OrderByDescending(m => m.EndDate.HasValue)
                    .ThenByDescending(m => m.EndDate)
                    .ToList()
                : Array.Empty<ExperienceTranslationsModel>();

        _loaded = true;
    }
}
