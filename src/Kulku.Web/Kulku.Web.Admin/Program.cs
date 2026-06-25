using Kulku.Application;
using Kulku.Domain;
using Kulku.Infrastructure;
using Kulku.Presentation.AspNetCore.Logging;
using Kulku.Web.Admin;
using Kulku.Web.Admin.Components;
using Kulku.Web.Admin.Components.Account;
using Kulku.Web.Admin.Endpoints;
using Kulku.Web.Admin.Options;
using Microsoft.Extensions.Options;
using Serilog;
using SoulNETLib.Clean.Infrastructure.Security;

const string applicationName = "Kulku.Admin";
Log.Logger = SerilogExtensions.CreateBootstrapLogger(applicationName);

try
{
    Log.Information("Starting Kulku Admin");

    var builder = WebApplication.CreateBuilder(args);

    // Add docker secrets to configuration for deployments
    builder.Configuration.AddDockerSecrets(
        new Dictionary<string, string>
        {
            { "ConnectionStrings:DefaultConnection", "kulku-default-conn" },
            { "ConnectionStrings:UserConnection", "kulku-user-conn" },
        }
    );

    builder.AddPresentationSerilog(applicationName);

    // Bind options from configuration
    builder
        .Services.AddAdminOptions()
        .AddAdminCore()
        .AddAdminAuthentication()
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddAssets();

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
    app.UseForwardedHeaders();
    app.UseHttpsRedirection();

    app.UsePresentationRequestLogging();

    app.UseAntiforgery();
    app.UseAssetStaticFiles();

    app.MapStaticAssets();
    app.MapCultureEndpoints();
    app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

    // Add additional endpoints required by the Identity /Account Razor components.
    app.MapAdditionalIdentityEndpoints();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Kulku Admin terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
