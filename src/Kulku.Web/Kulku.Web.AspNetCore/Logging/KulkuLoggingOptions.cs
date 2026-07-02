namespace Kulku.Web.AspNetCore.Logging;

public sealed class KulkuLoggingOptions
{
    public const string SectionName = "LoggingOptions";

    /// <summary>
    /// Write log events to standard output.
    /// Defaults to <see langword="true"/> because container runtimes collect stdout as the primary log stream.
    /// Set to <see langword="false"/> when file logging is the sole output and console output is undesirable.
    /// </summary>
    public bool WriteToConsole { get; init; } = true;

    public bool WriteToFile { get; init; }

    public string LogDirectory { get; init; } = "logs";

    public string FileNamePrefix { get; init; } = "kulku";

    public int RetainedFileCountLimit { get; init; } = 14;

    public long? FileSizeLimitBytes { get; init; } = 10 * 1024 * 1024;
}
