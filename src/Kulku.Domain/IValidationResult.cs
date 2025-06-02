namespace Kulku.Domain;

/// <summary>
/// Represents the outcome of a validation operation,
/// encapsulating one or more <see cref="AppError"/>s when validation fails.
/// </summary>
public interface IValidationResult
{
    /// <summary>
    /// A default <see cref="AppError"/> indicating a general validation failure.
    /// </summary>
    public static readonly AppError ValidationError = new(
        ErrorCodes.Validation,
        "A validation problem occured."
    );

    /// <summary>
    /// Gets the collection of validation errors that caused the result to be invalid.
    /// </summary>
    IEnumerable<AppError> Errors { get; }
}
