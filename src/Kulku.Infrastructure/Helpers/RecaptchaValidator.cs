using System.Net.Http.Json;
using Kulku.Application.Helpers;
using Microsoft.Extensions.Options;

namespace Kulku.Infrastructure.Helpers;

/// <summary>
/// Validates reCAPTCHA tokens by calling the Google reCAPTCHA verification endpoint.
/// </summary>
public class RecaptchaValidator(HttpClient httpClient, IOptions<RecaptchaOptions> options)
    : IRecaptchaValidator
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _secret = options.Value.SecretKey;

    /// <inheritdoc />
    public async Task<bool> ValidateAsync(
        string token,
        CancellationToken cancellationToken = default
    )
    {
        var uri = new Uri(
            $"https://www.google.com/recaptcha/api/siteverify?secret={_secret}&response={token}"
        );
        var response = await _httpClient.PostAsync(uri, null, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return false;

        var content = await response.Content.ReadFromJsonAsync<RecaptchaResponse>(
            cancellationToken: cancellationToken
        );
        return content?.Success ?? false;
    }

    /// <summary>
    /// Represents the structure of the response from Google's reCAPTCHA verification endpoint.
    /// </summary>
    private record RecaptchaResponse(bool Success);
}

/// <summary>
/// Holds the configuration options for reCAPTCHA verification.
/// </summary>
public class RecaptchaOptions
{
    /// <summary>
    /// The secret key used to authenticate with the Google reCAPTCHA API.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
}
