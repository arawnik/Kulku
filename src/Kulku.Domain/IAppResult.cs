namespace Kulku.Domain;

/// <summary>
/// Base interface for all result types (generic and non-generic).
/// Allows both <see cref="AppResult"/> and <see cref="AppResult{T}"/> to be stored together.
/// </summary>
public interface IAppResult
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// The error associated with a failed operation.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Naming",
        "CA1716:Identifiers should not match keywords",
        Justification = "Error is still the simplest way to describe it"
    )]
    AppError? Error { get; }
}
