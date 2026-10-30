using System.Net.Http.Json;
using System.Text.Json;
using Vikingos.Shared;

namespace Vikingos.Web.Services;

/// <summary>
/// Consume la Web API con HttpClient y convierte cualquier fallo
/// (servidor apagado, 400, 404, 409, 500, JSON inválido) en un mensaje entendible.
/// </summary>
public class VikingoApiClient(HttpClient http) : IVikingoApiClient
{
    private const string Ruta = "api/vikingos";

    public Task<ApiResult<List<VikingoResponse>>> ObtenerTodosAsync() =>
        EnviarAsync<List<VikingoResponse>>(() => http.GetAsync(Ruta));

    public Task<ApiResult<VikingoResponse>> CrearAsync(VikingoRequest request) =>
        EnviarAsync<VikingoResponse>(() => http.PostAsJsonAsync(Ruta, request));

    public Task<ApiResult<VikingoResponse>> ActualizarAsync(Guid id, VikingoRequest request) =>
        EnviarAsync<VikingoResponse>(() => http.PutAsJsonAsync($"{Ruta}/{id}", request));

    public async Task<ApiResult<bool>> EliminarAsync(Guid id)
    {
        var resultado = await EnviarAsync<object>(() => http.DeleteAsync($"{Ruta}/{id}"), leerCuerpo: false);
        return resultado.Exito ? ApiResult<bool>.Ok(true) : ApiResult<bool>.Fallo(resultado.Error!);
    }

    private static async Task<ApiResult<T>> EnviarAsync<T>(Func<Task<HttpResponseMessage>> peticion, bool leerCuerpo = true)
    {
        try
        {
            using var respuesta = await peticion();

            if (!respuesta.IsSuccessStatusCode)
            {
                return ApiResult<T>.Fallo(await LeerErrorAsync(respuesta));
            }

            return ApiResult<T>.Ok(leerCuerpo ? await respuesta.Content.ReadFromJsonAsync<T>() : default);
        }
        catch (HttpRequestException)
        {
            return ApiResult<T>.Fallo("No fue posible conectarse con la API. Verifique que esté en ejecución.");
        }
        catch (JsonException)
        {
            return ApiResult<T>.Fallo("La API devolvió una respuesta con formato inválido.");
        }
        catch (TaskCanceledException)
        {
            return ApiResult<T>.Fallo("La API tardó demasiado en responder.");
        }
    }

    /// <summary>Lee el ProblemDetails que devuelve la API y arma un mensaje para el usuario.</summary>
    private static async Task<string> LeerErrorAsync(HttpResponseMessage respuesta)
    {
        try
        {
            var problema = await respuesta.Content.ReadFromJsonAsync<ProblemaApi>();
            if (problema?.Errors is { Count: > 0 })
            {
                return string.Join(" ", problema.Errors.SelectMany(e => e.Value));
            }

            if (!string.IsNullOrWhiteSpace(problema?.Detail ?? problema?.Title))
            {
                return problema!.Detail ?? problema.Title!;
            }
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            // El cuerpo no era JSON; se usa el mensaje genérico de abajo.
        }

        return $"La API respondió con error {(int)respuesta.StatusCode} ({respuesta.ReasonPhrase}).";
    }

    private sealed record ProblemaApi(string? Title, string? Detail, Dictionary<string, string[]>? Errors);
}
