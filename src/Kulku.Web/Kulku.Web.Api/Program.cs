using Carter;
using Kulku.Application;
using Kulku.Domain;
using Kulku.Infrastructure;
using Kulku.Web.Api;
using SoulNETLib.Clean.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Add docker secrets to configuration for deployments
builder.Configuration.AddDockerSecrets(
    new Dictionary<string, string>
    {
        { "ConnectionStrings:DefaultConnection", "kulku-default-conn" },
        { "ConnectionStrings:UserConnection", "kulku-user-conn" },
        { "Recaptcha:SecretKey", "kulku-recaptcha-secret" },
    }
);

// Bind options and register services
builder
    .Services.AddApiCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApiCors(builder.Configuration)
    .AddApiOpenApi();

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

app.MapCarter();

await app.RunAsync();
