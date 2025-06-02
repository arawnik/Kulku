namespace Kulku.Domain;

/// <summary>
/// Represents an error in an operation.
/// </summary>
public sealed record AppError(string Code, string Message)
{
    #region ErrorType methods

    /// <summary>
    /// Creates a "Not Found" error.
    /// </summary>
    public static AppError NotFound(string message = "The requested resource was not found.") =>
        new(ErrorCodes.NotFound, message);

    /// <summary>
    /// Creates a "Not Found" error.
    /// </summary>
    public static AppError NotFound<TKey>(string type, TKey key) =>
        new(ErrorCodes.NotFound, $"{type} with ID {key} not found.");

    /// <summary>
    /// Creates a business rule violation error. These errors are typically issues that would lead to invalid state.
    /// </summary>
    public static AppError BusinessRule(string message) => new(ErrorCodes.BusinessRule, message);

    /// <summary>
    /// Creates an invalid data error. These errors are typically issue that rose from database.
    /// </summary>
    public static AppError InvalidData(string message) => new(ErrorCodes.InvalidData, message);

    #endregion

    #region Helper methods

    /// <summary>
    /// Creates an error from an exception.
    /// </summary>
    public static AppError FromException(Exception exception, string code = ErrorCodes.General) =>
        new(code, exception?.Message ?? string.Empty);

    #endregion

    /// <summary>
    /// Converts the error into a failed <see cref="AppResult"/>.
    /// </summary>
    public AppResult ToResult() => AppResult.Failure(this);

    /// <summary>
    /// Converts the error into a failed <see cref="AppResult{T}"/>.
    /// </summary>
    public AppResult<T> ToResult<T>() => AppResult<T>.Failure(this);
}
