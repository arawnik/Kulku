using Kulku.Application;
using Kulku.Domain;
using Kulku.Infrastructure;
using Kulku.Infrastructure.Security;
using Kulku.Web.Admin;
using Kulku.Web.Admin.Components;
using Kulku.Web.Admin.Components.Account;
using Kulku.Web.Admin.Components.Developer;
using Kulku.Web.Admin.Options;
using Microsoft.Extensions.Options;

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

// Bind options from configuration
builder
    .Services.AddAdminOptions()
    .AddAdminCore()
    .AddAdminAuthentication()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddAssets();

//TODO: Remove when crm has actual implementation.
builder.Services.AddSingleton<CrmProtoStore>();
builder.Services.AddScoped<CrmService>();

var app = builder.Build();

var managementOptions = app.Services.GetRequiredService<IOptions<ManagementOptions>>().Value;
if (managementOptions.MigrateOnStart)
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
app.UseAssetStaticFiles();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await app.RunAsync();
