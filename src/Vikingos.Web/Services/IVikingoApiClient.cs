using Vikingos.Shared;

namespace Vikingos.Web.Services;

/// <summary>Cliente HTTP de la API de vikingos.</summary>
public interface IVikingoApiClient
{
    Task<ApiResult<List<VikingoResponse>>> ObtenerTodosAsync();
    Task<ApiResult<VikingoResponse>> CrearAsync(VikingoRequest request);
    Task<ApiResult<VikingoResponse>> ActualizarAsync(Guid id, VikingoRequest request);
    Task<ApiResult<bool>> EliminarAsync(Guid id);
}
