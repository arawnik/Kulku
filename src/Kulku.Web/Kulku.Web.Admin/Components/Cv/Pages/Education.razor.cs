using Kulku.Application.Cover;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;

namespace Kulku.Web.Admin.Components.Cv.Pages;

partial class Education(
    IQueryHandler<
        GetEducationTranslations.Query,
        IReadOnlyList<EducationTranslationsModel>
    > translationsHandler
)
{
    private IReadOnlyList<EducationTranslationsModel> Educations { get; set; } = [];
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
        var tresult = await translationsHandler.Handle(
            new GetEducationTranslations.Query(),
            CancellationToken
        );
        var models = tresult.IsSuccess && tresult.Value is not null ? tresult.Value : [];

        Educations =
        [
            .. models.OrderByDescending(m => m.EndDate.HasValue).ThenByDescending(m => m.EndDate),
        ];

        _loaded = true;
    }
}
