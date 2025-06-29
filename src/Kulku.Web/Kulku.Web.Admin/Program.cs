using Kulku.Application;
using Kulku.Infrastructure;
using Kulku.Infrastructure.Helpers;
using Kulku.Persistence;
using Kulku.Persistence.Data;
using Kulku.Web.Admin.Components;
using Kulku.Web.Admin.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
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
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
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

builder
    .Services.AddIdentityCore<ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = true
    )
    .AddEntityFrameworkStores<UserDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    }
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.RunMigrations();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseForwardedHeaders();

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    //TODO: Remove from non dev environments when alternative is set up!
    //await app.RunMigrations();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await app.RunAsync();
