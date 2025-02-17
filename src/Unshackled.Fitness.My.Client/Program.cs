using System.Reflection;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.My.Client.Models;
using Unshackled.Fitness.My.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

ClientConfiguration clientConfig = new();
builder.Configuration.GetSection("ClientConfiguration").Bind(clientConfig);
builder.Services.AddSingleton(clientConfig);

StorageSettings storageSettings = new();
builder.Configuration.GetSection("StorageSettings").Bind(storageSettings);
builder.Services.AddSingleton(storageSettings);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddTransient<CookieHandler>();

// Local API Calls
builder.Services
    .AddHttpClient(Globals.ApiConstants.LocalApi, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CookieHandler>()
	.AddHttpMessageHandler<HttpStatusCodeHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient(Globals.ApiConstants.LocalApi));

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
	Assembly.GetExecutingAssembly(),
	typeof(AppState).Assembly
]));

builder.Services.AddSingleton<IRenderStateService, RenderStateService>();
builder.Services.AddSingleton<IScreenWakeLockService, ScreenWakeLockService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<HttpStatusCodeHandler>();
builder.Services.AddScoped<ILocalStorage, LocalStorage>();

await builder.Build().RunAsync();
