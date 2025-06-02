namespace Kulku.Domain;

/// <summary>
/// A generic validation result type that represents a failure and
/// carries one or more <see cref="AppError"/> entries, without a successful value.
/// </summary>
/// <typeparam name="TValue">The type parameter for the result value (unused on failure).</typeparam>
/// <remarks>
/// Use <see cref="WithErrors(AppError[])"/> to create an instance containing
/// the specific validation errors encountered.
/// </remarks>
public sealed class ValidationResult<TValue> : AppResult<TValue>, IValidationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResult{TValue}"/> class
    /// with the specified validation errors.
    /// </summary>
    /// <param name="errors">An array of <see cref="AppError"/> describing each validation failure.</param>
    private ValidationResult(AppError[] errors)
        : base(IValidationResult.ValidationError) => Errors = errors;

    /// <inheritdoc/>
    public IEnumerable<AppError> Errors { get; } = [];

    /// <summary>
    /// Creates a <see cref="ValidationResult{TValue}"/> containing the given validation errors.
    /// </summary>
    /// <param name="errors">An array of <see cref="AppError"/> instances to include in the result.</param>
    /// <returns>
    /// A new <see cref="ValidationResult{TValue}"/> populated with the provided errors,
    /// without a valid <typeparamref name="TValue"/> value.
    /// </returns>
    public static ValidationResult<TValue> WithErrors(AppError[] errors) => new(errors);
}
