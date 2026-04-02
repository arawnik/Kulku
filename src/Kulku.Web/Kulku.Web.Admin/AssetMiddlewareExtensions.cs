using Kulku.Application.Abstractions.Assets;
using Kulku.Infrastructure.Assets;
using Microsoft.Extensions.FileProviders;

namespace Kulku.Web.Admin;

/// <summary>
/// Middleware extensions for serving local asset files.
/// </summary>
public static class AssetMiddlewareExtensions
{
    /// <summary>
    /// Conditionally registers static file middleware when the asset builder resolved
    /// to local mode. Does nothing when assets are served from an external URL.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for chaining.</returns>
    public static IApplicationBuilder UseAssetStaticFiles(this IApplicationBuilder app)
    {
        var builder = (AssetUrlBuilder)
            app.ApplicationServices.GetRequiredService<IAssetUrlBuilder>();

        if (builder.LocalAssetPath is not null)
        {
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(builder.LocalAssetPath),
                    RequestPath = "/static",
                }
            );
        }

        return app;
    }
}
