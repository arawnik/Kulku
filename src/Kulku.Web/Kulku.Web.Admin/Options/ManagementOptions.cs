namespace Kulku.Web.Admin.Options;

public sealed class ManagementOptions
{
    public const string SectionName = "Management";

    /// <summary>
    /// Whether the migrations will be ran on startup. Defaults to <c>false</c>.
    /// </summary>
    public required bool MigrateOnStart { get; set; }

    /// <summary>
    /// Whether the user registration form is enabled. Defaults to <c>false</c>.
    /// </summary>
    public bool RegistrationEnabled { get; set; }

    /// <summary>
    /// URL to the public contact form. Shown on the landing page when registration is closed.
    /// </summary>
    public string? ContactUrl { get; set; }
}
