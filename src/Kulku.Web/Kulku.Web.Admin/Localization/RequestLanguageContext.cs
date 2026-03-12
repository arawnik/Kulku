using System.Globalization;
using Kulku.Application.Abstractions.Localization;
using Kulku.Domain;
using Microsoft.AspNetCore.Localization;

namespace Kulku.Web.Admin.Localization;

/// <summary>
/// Resolves the current <see cref="LanguageCode"/> in a Blazor-aware manner.
///
/// Prioritizes the request localization feature when an HTTP context is available
/// (useful for the initial server request), but falls back to the ambient
/// <see cref="CultureInfo.CurrentUICulture"/> which Blazor sets per-circuit.
/// </summary>
public sealed class RequestLanguageContext(IHttpContextAccessor? httpContextAccessor = null)
    : ILanguageContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor = httpContextAccessor;

    public LanguageCode Current
    {
        get
        {
            // If an HTTP context exists and the localization middleware selected a culture,
            // prefer that mapping. This handles cases like the initial HTTP request.
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext is not null)
            {
                var feature = httpContext.Features.Get<IRequestCultureFeature>();
                var culture = feature?.RequestCulture.UICulture;
                if (culture is not null)
                    return LanguageCodeMapper.FromCulture(culture);
            }

            // Blazor Server and WebAssembly set the ambient culture for the circuit/runtime.
            // Use that as the default in UI scenarios.
            return LanguageCodeMapper.FromCulture(CultureInfo.CurrentUICulture);
        }
    }
}
