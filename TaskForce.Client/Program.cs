using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskForce.Client;
using TaskForce.Client.EventAggregator;
using TaskForce.Client.Services;
using TaskForceSdk;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
if (string.IsNullOrEmpty(apiBaseUrl))
{
    throw new InvalidDataException("AppSettings:ApiBaseUrl is null or empty");
}

var siteUrl = builder.Configuration["SiteUrl"];
if (string.IsNullOrEmpty(apiBaseUrl))
{
    throw new InvalidDataException("AppSettings:SiteUrl is null or empty");
}


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<StateContainerService>();
builder.Services.AddTransient<HttpErrorMessageHandler>();


builder.Services.AddHttpClient<FasiProgettoClient>(a =>
{
    var uri = new Uri(apiBaseUrl);
    a.BaseAddress = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");
}).AddHttpMessageHandler<HttpErrorMessageHandler>().RemoveAllLoggers();

builder.Services.AddHttpClient<MacroFasiClient>(a =>
{
    var uri = new Uri(apiBaseUrl);
    a.BaseAddress = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");
}).AddHttpMessageHandler<HttpErrorMessageHandler>().RemoveAllLoggers();

builder.Services.AddHttpClient<PauseClient>(a =>
{
    var uri = new Uri(apiBaseUrl);
    a.BaseAddress = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");
}).AddHttpMessageHandler<HttpErrorMessageHandler>().RemoveAllLoggers();

builder.Services.AddHttpClient<PreseInCaricoClient>(a =>
{
    var uri = new Uri(apiBaseUrl);
    a.BaseAddress = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");
}).AddHttpMessageHandler<HttpErrorMessageHandler>().RemoveAllLoggers();

builder.Services.AddHttpClient<ProgettiClient>(a =>
{
    var uri = new Uri(apiBaseUrl);
    a.BaseAddress = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");
}).AddHttpMessageHandler<HttpErrorMessageHandler>().RemoveAllLoggers();

builder.Services.AddHttpClient<UsersClient>(a =>
{
    var uri = new Uri(apiBaseUrl);
    a.BaseAddress = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");
}).AddHttpMessageHandler<HttpErrorMessageHandler>().RemoveAllLoggers();

builder.Services.AddHttpClient<UsersFasiClient>(a =>
{
    var uri = new Uri(apiBaseUrl);
    a.BaseAddress = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");
}).AddHttpMessageHandler<HttpErrorMessageHandler>().RemoveAllLoggers();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

//var culture = new CultureInfo("it");
//CultureInfo.DefaultThreadCurrentCulture = culture;
//CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddScoped<SdkService>();


await builder.Build().RunAsync();
