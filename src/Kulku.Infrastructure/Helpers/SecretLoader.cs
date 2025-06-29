namespace Kulku.Infrastructure.Helpers;

public static class SecretLoader
{
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
