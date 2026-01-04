using Carter;
using Kulku.Application;
using Kulku.Application.Abstractions.Localization;
using Kulku.Domain;
using Kulku.Infrastructure;
using Kulku.Infrastructure.Security;
using Kulku.Web.Api.Localization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add docker secrets to configuration for deployments
SecretLoader.LoadFileSecretsIntoConfiguration(
    builder.Configuration,
    new Dictionary<string, string>
    {
        { "ConnectionStrings:DefaultConnection", "kulku-default-conn" },
        { "ConnectionStrings:UserConnection", "kulku-user-conn" },
        { "Recaptcha:SecretKey", "kulku-recaptcha-secret" },
    }
);

// Add services to the container.

builder.Services.AddLocalization();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddProblemDetails();
builder.Services.AddCarter();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILanguageContext, RequestLanguageContext>();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "DefaultCors",
        policy =>
        {
            // Restrictive policy: only configured origins allowed.
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
        }
    );
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            // Permissive policy: allow all origins (for development).
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
    exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context))
);

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
    app.UseCors("AllowAll");
}
else
{
    app.MapOpenApi().RequireAuthorization();
    app.UseCors("DefaultCors");

    app.UseForwardedHeaders(
        new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        }
    );
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapCarter();

await app.RunAsync();
