using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components.Pages;

[SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not match keywords",
    Justification = "The error page comes built in with the naming and makes the most sense as is in context."
)]
partial class Error
{
    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    private string? RequestId { get; set; }
    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    protected override void OnInitialized() =>
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
}
