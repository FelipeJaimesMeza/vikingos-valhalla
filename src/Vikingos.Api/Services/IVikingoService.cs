using Vikingos.Shared;

namespace Vikingos.Api.Services;

/// <summary>Casos de uso del CRUD de vikingos.</summary>
public interface IVikingoService
{
    Task<IReadOnlyList<VikingoResponse>> ObtenerTodosAsync();
    Task<VikingoResponse?> ObtenerPorIdAsync(Guid id);
    Task<VikingoResponse> CrearAsync(VikingoRequest request);
    Task<VikingoResponse?> ActualizarAsync(Guid id, VikingoRequest request);
    Task<bool> EliminarAsync(Guid id);
}
