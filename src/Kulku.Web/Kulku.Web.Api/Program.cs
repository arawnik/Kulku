using Carter;
using Kulku.Application;
using Kulku.Application.Abstractions.Localization;
using Kulku.Domain;
using Kulku.Infrastructure;
using Kulku.Infrastructure.Helpers;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
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

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async httpContext =>
    {
        var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
        if (pds == null || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
        {
            // Fallback behavior
            await httpContext.Response.WriteAsync("Fallback: An error occurred.");
        }
    });
});

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    }
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
}
else
{
    app.MapOpenApi().RequireAuthorization();
    app.UseExceptionHandler();

    app.UseForwardedHeaders();
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.MapCarter();

await app.RunAsync();
