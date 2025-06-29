using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kulku.Infrastructure.Helpers;

public static class SecretLoader
{
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
        const string secretDir = "/run/secrets";

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
                Console.WriteLine(
                    $"[SecretLoader] Loaded secret '{secretFile}' into configuration key '{configKey}'."
                );
            }
            else
            {
                Console.WriteLine(
                    $"[SecretLoader] Secret file '{secretFile}' not found at '{path}'."
                );
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
        const string secretDir = "/run/secrets";

        foreach (var kv in envVarToSecretFileName)
        {
            var envVar = kv.Key;
            var secretFile = kv.Value;
            var path = Path.Combine(secretDir, secretFile);

            if (File.Exists(path))
            {
                var value = File.ReadAllText(path).Trim();
                Environment.SetEnvironmentVariable(envVar, value);
                Console.WriteLine(
                    $"[SecretLoader] Loaded secret '{secretFile}' into env-var '{envVar}'."
                );
            }
            else
            {
                Console.WriteLine($"[SecretLoader] Secret file not found: {path}");
            }
        }
    }
}
