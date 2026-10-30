using Vikingos.Api.Domain;
using Vikingos.Api.Infrastructure;
using Vikingos.Api.Repositories;
using Vikingos.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Manejo de errores: toda excepción se convierte en ProblemDetails (JSON estándar).
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Inyección de dependencias (la "D" de SOLID): se registran interfaces, no clases concretas.
// Singleton en el repositorio para que los datos en memoria vivan mientras corre la API.
builder.Services.AddSingleton<IVikingoRepository, InMemoryVikingoRepository>();
builder.Services.AddSingleton<IValhallaCalculator, ValhallaCalculator>();
builder.Services.AddScoped<IVikingoService, VikingoService>();

// CORS: permite que el Blazor (otro puerto = otro origen) llame a la API desde el navegador.
var origenesPermitidos = builder.Configuration.GetSection("Cors:Origenes").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
    options.AddPolicy("BlazorClient", policy =>
        policy.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("BlazorClient");
app.MapControllers();

await DatosIniciales.CargarAsync(app.Services.GetRequiredService<IVikingoRepository>());

app.Run();

// Permite usar WebApplicationFactory<Program> en pruebas de integración.
public partial class Program;
