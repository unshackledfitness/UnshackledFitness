using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor;
using MudBlazor.Services;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Services;
using Unshackled.Fitness.My.Components;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Fitness.My.Middleware;
using Unshackled.Fitness.My.Services;
using Unshackled.Fitness.My.Utils;

var builder = WebApplication.CreateBuilder(args);

ConnectionStrings connectionStrings = new ConnectionStrings
{
	DefaultDatabase = builder.Configuration.GetConnectionString("DefaultDatabase") ?? string.Empty
};
builder.Services.AddSingleton(connectionStrings);

SiteConfiguration siteConfig = new();
builder.Configuration.GetSection("SiteConfiguration").Bind(siteConfig);
builder.Services.AddSingleton(siteConfig);

HashIdConfiguration hashConfig = new();
builder.Configuration.GetSection("HashIdConfiguration").Bind(hashConfig);
HashIdSettings.Configure(hashConfig.Alphabet, hashConfig.Salt, hashConfig.MinLength);

SmtpSettings smtpSettings = new();
builder.Configuration.GetSection("SmtpSettings").Bind(smtpSettings);
builder.Services.AddSingleton(smtpSettings);

DbConfiguration dbConfig = new();
builder.Configuration.GetSection("DbConfiguration").Bind(dbConfig);
builder.Services.AddSingleton(dbConfig);

StorageSettings storageSettings = new();
builder.Configuration.GetSection("StorageSettings").Bind(storageSettings);
builder.Services.AddSingleton(storageSettings);

AuthenticationProviderConfiguration authProviderConfig = new();
builder.Configuration.GetSection("AuthenticationProviders").Bind(authProviderConfig);

switch (dbConfig.DatabaseType?.ToLower())
{
	case DbConfiguration.MSSQL:
		builder.Services.AddDbContext<BaseDbContext, MsSqlServerDbContext>();
		break;
	case DbConfiguration.MYSQL:
		builder.Services.AddDbContext<BaseDbContext, MySqlServerDbContext>();
		break;
	case DbConfiguration.POSTGRESQL:
		builder.Services.AddDbContext<BaseDbContext, PostgreSqlServerDbContext>();
		break;
	default:
		break;
}
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

if (!builder.Environment.IsDevelopment())
{
	builder.Services.AddDataProtection()
			.SetApplicationName("UnshackledFitness")
			.PersistKeysToDbContext<BaseDbContext>();
}

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options => {
		options.DefaultScheme = IdentityConstants.ApplicationScheme;
		options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
	})
	.AddMicrosoftAccount(authProviderConfig)
	.AddGoogleAccount(authProviderConfig)
	.AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(o =>
{
	o.LoginPath = "/account/login";
	o.LogoutPath = "/account/logout";
	o.ExpireTimeSpan = TimeSpan.FromDays(14);
	o.SlidingExpiration = true;
	o.Cookie.Name = siteConfig.SiteName?.RemoveNonAlphaNumeric() ?? "UnshackledFitness";
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));

builder.Services.AddIdentityCore<UserEntity>(options => {
	options.SignIn.RequireConfirmedAccount = siteConfig.RequireConfirmedAccount;
	options.Password.RequireDigit = siteConfig.PasswordStrength.RequireDigit;
	options.Password.RequireLowercase = siteConfig.PasswordStrength.RequireLowercase;
	options.Password.RequireNonAlphanumeric = siteConfig.PasswordStrength.RequireNonAlphanumeric;
	options.Password.RequireUppercase = siteConfig.PasswordStrength.RequireUppercase;
	options.Password.RequiredLength = siteConfig.PasswordStrength.RequiredLength;
	options.Password.RequiredUniqueChars = siteConfig.PasswordStrength.RequiredUniqueChars;
})
	.AddEntityFrameworkStores<BaseDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

builder.Services.AddRazorComponents()
	.AddInteractiveWebAssemblyComponents()
	.AddAuthenticationStateSerialization();

builder.Services.AddControllers();

builder.Services.AddMudServices(config =>
{
	config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

	config.SnackbarConfiguration.PreventDuplicates = false;
	config.SnackbarConfiguration.NewestOnTop = false;
	config.SnackbarConfiguration.ShowCloseIcon = true;
	config.SnackbarConfiguration.VisibleStateDuration = 3000;
	config.SnackbarConfiguration.HideTransitionDuration = 200;
	config.SnackbarConfiguration.ShowTransitionDuration = 200;
	config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([
	Assembly.GetExecutingAssembly()
]));
builder.Services.AddAutoMapper([
	Assembly.GetExecutingAssembly()
]);
builder.Services.AddValidatorsFromAssemblies([
	Assembly.GetExecutingAssembly()
]);

builder.Services.TryAddScoped<IWebAssemblyHostEnvironment, ServerHostEnvironment>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRenderStateService, Unshackled.Fitness.My.Services.RenderStateService>();
builder.Services.AddSingleton<IEmailSender<UserEntity>, SmtpService>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

using var scope = app.Services.CreateScope();
bool isReady = await DbStartupService.IsReady(dbConfig, connectionStrings, scope.ServiceProvider);

if (!isReady)
	throw new Exception("Unable to connect to the database.");

if (siteConfig.ApplyMigrationsOnStartup)
{
	await DbStartupService.ApplyMigrations(dbConfig, connectionStrings, scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseAntiforgery();

app.UseMiddleware<AuthorizedUserMiddleware>();
app.UseMiddleware<AuthorizedMemberMiddleware>();

app.MapRazorComponents<App>()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(Unshackled.Fitness.My.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapControllers();

app.Run();
