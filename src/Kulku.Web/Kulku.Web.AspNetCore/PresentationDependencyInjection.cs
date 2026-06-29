using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Kulku.Web.AspNetCore;

[ExcludeFromCodeCoverage]
public static class PresentationDependencyInjection
{
    /// <summary>
    /// Registers core presentation services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddPresentationCore(this IServiceCollection services)
    {
        services
            .Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor
                    | ForwardedHeaders.XForwardedProto
                    | ForwardedHeaders.XForwardedHost;

                // Clear defaults so any reverse proxy (NPM, nginx) on a non-loopback network is trusted.
                // Safe as long as the app port is not publicly exposed.
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            })
            .AddLocalization()
            .AddHttpContextAccessor();

        return services;
    }
}
