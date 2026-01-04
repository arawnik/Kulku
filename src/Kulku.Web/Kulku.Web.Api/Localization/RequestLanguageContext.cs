using System.Globalization;
using Kulku.Application.Abstractions.Localization;
using Kulku.Domain;
using Microsoft.AspNetCore.Localization;

namespace Kulku.Web.Api.Localization;

/// <summary>
/// Resolves the current <see cref="LanguageCode"/> from ASP.NET Core request localization.
/// </summary>
/// <remarks>
/// This implementation relies on <see cref="RequestLocalizationMiddleware"/> having already
/// selected the request culture. It performs a small mapping from the request's UI culture
/// to the domain language enum.
/// </remarks>
public sealed class RequestLanguageContext(IHttpContextAccessor httpContextAccessor)
    : ILanguageContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public LanguageCode Current
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
                return Defaults.Language;

            var feature = httpContext.Features.Get<IRequestCultureFeature>();
            var culture = feature?.RequestCulture.UICulture ?? CultureInfo.CurrentUICulture;

            return LanguageCodeMapper.FromCulture(culture);
        }
    }
}
