namespace Kulku.Web.Admin.Options;

public sealed class ManagementOptions
{
    public const string SectionName = "Management";

    public required bool MigrateOnStart { get; set; }
}
