using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not match keywords",
    Justification = "Blazor convention for shared components folder",
    Scope = "namespace",
    Target = "~N:Kulku.Web.Admin.Components.Shared"
)]
