using Carter;
using Kulku.Application;
using Kulku.Domain;
using Kulku.Infrastructure;
using Kulku.Web.Api;
using Kulku.Web.AspNetCore;
using Kulku.Web.AspNetCore.Logging;
using Serilog;
using SoulNETLib.Clean.Infrastructure.Security;

const string applicationName = "Kulku.Api";
Log.Logger = SerilogExtensions.CreateBootstrapLogger(applicationName);

try
{
    Log.Information("Starting Kulku Api");

    var builder = WebApplication.CreateBuilder(args);

    // Add docker secrets to configuration for deployments
    builder.Configuration.AddDockerSecrets(
        new Dictionary<string, string>
        {
            { "ConnectionStrings:DefaultConnection", "default-conn" },
            { "ConnectionStrings:UserConnection", "user-conn" },
            { "Recaptcha:SecretKey", "recaptcha-secret" },
            { "Observability:OtlpHeaders", "otlp-headers" },
        }
    );

    builder.AddPresentationSerilog(applicationName);

    // Bind options and register services
    builder
        .Services.AddApiOptions()
        .AddApiCore()
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddApiCors(builder.Configuration)
        .AddApiOpenApi()
        .AddObservability(builder.Configuration, builder.Environment);

    var app = builder.Build();

    app.UseExceptionHandler(exceptionHandlerApp =>
        exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context))
    );

    app.UseForwardedHeaders();

    var localizationOptions = new RequestLocalizationOptions()
        .SetDefaultCulture(Defaults.Culture)
        .AddSupportedCultures(Defaults.SupportedCultures)
        .AddSupportedUICultures(Defaults.SupportedCultures);
    localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

    app.UseRequestLocalization(localizationOptions);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseCors(ApiCorsPolicyNames.AllowAll);
    }
    else
    {
        app.MapOpenApi().RequireAuthorization();
        app.UseCors(ApiCorsPolicyNames.Default);

        app.UseHsts();
        app.UseHttpsRedirection();
    }

    app.UsePresentationRequestLogging();

    app.MapCarter();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Kulku Api terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
