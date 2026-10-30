using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Vikingos.Web;
using Vikingos.Web.Services;
using Vikingos.Web.State;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// La URL de la API se lee de wwwroot/appsettings.json (no queda "quemada" en el código).
var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("Falta configurar 'ApiBaseUrl' en wwwroot/appsettings.json.");

// HttpClient tipado: se inyecta ya configurado dentro de VikingoApiClient.
builder.Services.AddHttpClient<IVikingoApiClient, VikingoApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
});

// Scoped en WebAssembly = una sola instancia por pestaña del navegador (estado compartido entre componentes).
builder.Services.AddScoped<VikingosState>();

await builder.Build().RunAsync();
