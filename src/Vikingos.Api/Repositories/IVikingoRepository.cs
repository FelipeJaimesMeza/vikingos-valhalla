using Vikingos.Api.Domain;

namespace Vikingos.Api.Repositories;

/// <summary>
/// Acceso a datos. Hoy es en memoria; mañana podría ser SQL Server sin tocar el servicio ni el controlador.
/// </summary>
public interface IVikingoRepository
{
    Task<IReadOnlyList<Vikingo>> ObtenerTodosAsync();
    Task<Vikingo?> ObtenerPorIdAsync(Guid id);
    Task<bool> ExisteNombreAsync(string nombre, Guid? excluirId = null);
    Task AgregarAsync(Vikingo vikingo);
    Task<bool> ActualizarAsync(Vikingo vikingo);
    Task<bool> EliminarAsync(Guid id);
}
