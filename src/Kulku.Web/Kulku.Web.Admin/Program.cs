using Kulku.Application;
using Kulku.Application.Abstractions.Localization;
using Kulku.Domain;
using Kulku.Infrastructure;
using Kulku.Infrastructure.Security;
using Kulku.Persistence;
using Kulku.Persistence.Data;
using Kulku.Web.Admin;
using Kulku.Web.Admin.Components;
using Kulku.Web.Admin.Components.Account;
using Kulku.Web.Admin.Components.Developer;
using Kulku.Web.Admin.Localization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add docker secrets to configuration for deployments
SecretLoader.LoadFileSecretsIntoConfiguration(
    builder.Configuration,
    new Dictionary<string, string>
    {
        { "ConnectionStrings:DefaultConnection", "kulku-default-conn" },
        { "ConnectionStrings:UserConnection", "kulku-user-conn" },
    }
);

// Add services to the container.

builder.Services.AddLocalization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<
    AuthenticationStateProvider,
    IdentityRevalidatingAuthenticationStateProvider
>();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ILanguageContext, RequestLanguageContext>();

builder
    .Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<UserDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

//TODO: Remove when crm has actual implementation.
builder.Services.AddSingleton<CrmProtoStore>();

var app = builder.Build();

var settings = app.Configuration.GetRequiredSection("Management").Get<ManagementSettings>();

if (settings?.MigrateOnStart == true)
{
    await app.RunMigrations();
}

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(Defaults.Culture)
    .AddSupportedCultures(Defaults.SupportedCultures)
    .AddSupportedUICultures(Defaults.SupportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

// Serve project images from the client's public directory when available (dev convenience).
// In production containers this directory won't exist, and the card falls back gracefully.
var clientProjectImages = Path.GetFullPath(
    Path.Combine(
        app.Environment.ContentRootPath,
        "..",
        "kulku.web.client",
        "public",
        "static",
        "projects"
    )
);
if (Directory.Exists(clientProjectImages))
{
    app.UseStaticFiles(
        new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                clientProjectImages
            ),
            RequestPath = "/static/projects",
        }
    );
}

var clientIntroductionImages = Path.GetFullPath(
    Path.Combine(
        app.Environment.ContentRootPath,
        "..",
        "kulku.web.client",
        "public",
        "static",
        "introductions"
    )
);
if (Directory.Exists(clientIntroductionImages))
{
    app.UseStaticFiles(
        new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                clientIntroductionImages
            ),
            RequestPath = "/static/introductions",
        }
    );
}

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await app.RunAsync();
