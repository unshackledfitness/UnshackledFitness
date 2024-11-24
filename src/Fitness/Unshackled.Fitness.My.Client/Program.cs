using System.Reflection;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Unshackled.Fitness.Core.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

ClientConfiguration clientConfig = new();
builder.Configuration.GetSection("ClientConfiguration").Bind(clientConfig);
builder.Services.AddSingleton(clientConfig);

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
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IAppState, AppState>();
builder.Services.AddScoped<HttpStatusCodeHandler>();
builder.Services.AddScoped<ILocalStorage, LocalStorage>();

await builder.Build().RunAsync();
