namespace Kulku.Domain;

/// <summary>
/// Represents the outcome of an operation that does not return a value.
/// </summary>
public class AppResult : IAppResult
{
    /// <inheritdoc/>
    public bool IsSuccess { get; }

    /// <inheritdoc/>
    public bool IsFailure => !IsSuccess;

    /// <inheritdoc/>
    public AppError? Error { get; }

    /// <summary>
    /// Initializes a result with success status and an optional error.
    /// </summary>
    protected AppResult(bool isSuccess, AppError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result without a value.
    /// </summary>
    public static AppResult Success() => new(true, null);

    /// <summary>
    /// Creates a successful result with a value, inferring the generic type automatically.
    /// </summary>
    public static AppResult<T> Success<T>(T value) => AppResult<T>.Success(value);

    /// <summary>
    /// Creates a failure result with an error.
    /// </summary>
    public static AppResult Failure(AppError error) => new(false, error);

    /// <summary>
    /// Creates a failure result from an exception.
    /// </summary>
    public static AppResult Failure(Exception exception) =>
        new(false, AppError.FromException(exception));

    /// <summary>
    /// Implicitly converts an error into a failure result.
    /// </summary>
    public static implicit operator AppResult(AppError error) => Failure(error);

    /// <summary>
    /// Implicitly converts an exception into a failure result.
    /// </summary>
    public static implicit operator AppResult(Exception exception) => Failure(exception);
}
