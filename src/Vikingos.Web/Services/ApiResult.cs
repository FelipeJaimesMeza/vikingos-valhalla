namespace Vikingos.Web.Services;

/// <summary>
/// Resultado de una llamada a la API: o trae datos, o trae un mensaje de error para el usuario.
/// Evita que las excepciones de red lleguen a los componentes.
/// </summary>
public record ApiResult<T>(bool Exito, T? Datos, string? Error)
{
    public static ApiResult<T> Ok(T? datos) => new(true, datos, null);
    public static ApiResult<T> Fallo(string error) => new(false, default, error);
}
