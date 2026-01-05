using Microsoft.Extensions.Configuration;

namespace Kulku.Infrastructure.Security;

/// <summary>
/// Utility class for loading file-based Docker secrets from `/run/secrets`
/// into your application's configuration or environment variables.
/// </summary>
public static class SecretLoader
{
    const string secretDir = "/run/secrets";

    /// <summary>
    /// Loads each secret-file under /run/secrets into the given configuration
    /// using colon-delimited keys (e.g. "ConnectionStrings:DefaultConnection").
    /// Must be called *before* any calls to GetConnectionString().
    /// </summary>
    public static void LoadFileSecretsIntoConfiguration(
        IConfiguration configuration,
        IDictionary<string, string> configKeyToSecretFileName
    )
    {
        foreach (var kv in configKeyToSecretFileName)
        {
            var configKey = kv.Key;
            var secretFile = kv.Value;
            var path = Path.Combine(secretDir, secretFile);

            if (File.Exists(path))
            {
                var value = File.ReadAllText(path).Trim();
                // **Directly override** the IConfiguration value:
                configuration[configKey] = value;
            }
        }
    }

    /// <summary>
    /// For each mapping of env-var name → secret-file name, if
    /// /run/secrets/{secretFileName} exists, reads its contents
    /// and calls Environment.SetEnvironmentVariable(envVarName, value).
    /// </summary>
    public static void LoadFileSecretsAsEnvironmentVariables(
        IDictionary<string, string> envVarToSecretFileName
    )
    {
        foreach (var kv in envVarToSecretFileName)
        {
            var envVar = kv.Key;
            var secretFile = kv.Value;
            var path = Path.Combine(secretDir, secretFile);

            if (File.Exists(path))
            {
                var value = File.ReadAllText(path).Trim();
                Environment.SetEnvironmentVariable(envVar, value);
            }
        }
    }
}
