using System.Globalization;
using Kulku.Domain;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Kulku.Web.Admin.Endpoints;

/// <summary>
/// Provides an endpoint for switching the application culture via cookie.
/// </summary>
public static class CultureEndpoint
{
    public static void MapCultureEndpoints(this WebApplication app)
    {
        app.MapPost(
                "/Culture/Set",
                ([FromForm] string culture, [FromForm] string returnUrl, HttpContext context) =>
                {
                    if (
                        !Defaults.SupportedCultures.Contains(
                            culture,
                            StringComparer.OrdinalIgnoreCase
                        )
                    )
                    {
                        culture = Defaults.Culture;
                    }

                    var cultureInfo = CultureInfo.GetCultureInfo(culture);
                    var cookieValue = CookieRequestCultureProvider.MakeCookieValue(
                        new RequestCulture(cultureInfo, cultureInfo)
                    );

                    context.Response.Cookies.Append(
                        CookieRequestCultureProvider.DefaultCookieName,
                        cookieValue,
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddYears(1),
                            IsEssential = true,
                            SameSite = SameSiteMode.Strict,
                        }
                    );

                    // Ensure returnUrl is local to prevent open redirect
                    if (string.IsNullOrEmpty(returnUrl) || !returnUrl.StartsWith('/'))
                    {
                        returnUrl = "/";
                    }

                    return Results.LocalRedirect(returnUrl);
                }
            )
            .DisableAntiforgery();
    }
}
