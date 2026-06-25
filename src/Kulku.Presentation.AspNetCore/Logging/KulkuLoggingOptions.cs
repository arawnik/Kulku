namespace Kulku.Presentation.AspNetCore.Logging;

public sealed class KulkuLoggingOptions
{
    public const string SectionName = "LoggingOptions";

    public bool WriteToConsole { get; init; } = true;

    public bool WriteToFile { get; init; }

    public string LogDirectory { get; init; } = "logs";

    public string FileNamePrefix { get; init; } = "kulku";

    public int RetainedFileCountLimit { get; init; } = 14;

    public long? FileSizeLimitBytes { get; init; } = 10 * 1024 * 1024;
}
