using Kulku.Web.Admin.Components.Developer;

namespace Kulku.Web.Admin.Components.Pages;

partial class Home
{
    private static DateTime MixCutoff => DateTime.Today.AddDays(-90);

    private int InboundTotal =>
        Store.Interactions.Count(i =>
            i.Direction == InteractionDirection.Inbound && i.Date >= MixCutoff
        );

    private int OutboundTotal =>
        Store.Interactions.Count(i =>
            i.Direction == InteractionDirection.Outbound && i.Date >= MixCutoff
        );

    private int ActiveIdeasCount =>
        Store.Ideas.Count(i =>
            i.Status is IdeaStatus.Seed or IdeaStatus.Exploring or IdeaStatus.Validating
        );
}
