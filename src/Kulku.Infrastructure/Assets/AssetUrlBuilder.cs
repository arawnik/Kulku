using Kulku.Application.Abstractions.Assets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Kulku.Infrastructure.Assets;

/// <summary>
/// Resolves asset serving strategy from <see cref="AssetOptions"/> at construction time.
/// Local path takes precedence; falls back to <see cref="AssetOptions.BaseUrl"/>.
/// </summary>
public sealed class AssetUrlBuilder : IAssetUrlBuilder
{
    private readonly string _baseUrl;

    /// <summary>
    /// Resolves the asset source from bound options.
    /// </summary>
    /// <param name="options">Bound <see cref="AssetOptions"/> from configuration.</param>
    /// <param name="environment">
    /// Hosting environment used to resolve relative paths against
    /// <see cref="IHostEnvironment.ContentRootPath"/>.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Neither a valid local path nor a base URL is configured.
    /// </exception>
    public AssetUrlBuilder(IOptions<AssetOptions> options, IHostEnvironment environment)
    {
        var opts = options.Value;

        if (!string.IsNullOrWhiteSpace(opts.LocalPath))
        {
            var resolved = Path.IsPathRooted(opts.LocalPath)
                ? opts.LocalPath
                : Path.GetFullPath(opts.LocalPath, environment.ContentRootPath);

            if (Directory.Exists(resolved))
            {
                _baseUrl = "/static";
                LocalAssetPath = resolved;
                return;
            }
        }

        if (!string.IsNullOrWhiteSpace(opts.BaseUrl))
        {
            _baseUrl = opts.BaseUrl.TrimEnd('/');
            return;
        }

        throw new InvalidOperationException(
            "No asset source configured. Set Assets:LocalPath or Assets:BaseUrl."
        );
    }

    /// <summary>
    /// Resolved absolute path to the local asset directory, or <c>null</c> when
    /// assets are served from an external URL.
    /// </summary>
    public string? LocalAssetPath { get; }

    /// <inheritdoc />
    public string Project(string fileName) => Build("projects", fileName);

    /// <inheritdoc />
    public string Introduction(string fileName) => Build("introductions", fileName);

    private string Build(string assetType, string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        return $"{_baseUrl}/{assetType}/{Uri.EscapeDataString(fileName)}";
    }
}
