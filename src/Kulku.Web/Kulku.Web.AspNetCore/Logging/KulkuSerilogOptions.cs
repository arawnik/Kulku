namespace Kulku.Web.AspNetCore.Logging;

public sealed class KulkuSerilogOptions
{
    public const string SectionName = "Serilog";

    public KulkuSerilogMinimumLevelOptions MinimumLevel { get; init; } = new();
}

public sealed class KulkuSerilogMinimumLevelOptions
{
    public string? Default { get; init; }

    public Dictionary<string, string> Override { get; init; } =
        new(StringComparer.OrdinalIgnoreCase);
}
