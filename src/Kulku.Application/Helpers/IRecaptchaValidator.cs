namespace Kulku.Application.Helpers;

/// <summary>
/// Validates reCAPTCHA tokens by calling the Google reCAPTCHA verification endpoint.
/// </summary>
public interface IRecaptchaValidator
{
    /// <summary>
    /// Verifies the provided token against the Google reCAPTCHA API.
    /// </summary>
    /// <param name="token">The reCAPTCHA token from the client.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>True if the token is valid; otherwise, false.</returns>
    Task<bool> ValidateAsync(string token, CancellationToken cancellationToken = default);
}
