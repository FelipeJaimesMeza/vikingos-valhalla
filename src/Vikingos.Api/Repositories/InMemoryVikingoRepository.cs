using System.Collections.Concurrent;
using Vikingos.Api.Domain;

namespace Vikingos.Api.Repositories;

/// <summary>
/// Guarda los vikingos en un diccionario en memoria.
/// ConcurrentDictionary porque la API atiende varias peticiones al mismo tiempo (es thread-safe).
/// </summary>
public class InMemoryVikingoRepository : IVikingoRepository
{
    private readonly ConcurrentDictionary<Guid, Vikingo> _vikingos = new();

    public Task<IReadOnlyList<Vikingo>> ObtenerTodosAsync()
    {
        IReadOnlyList<Vikingo> lista = _vikingos.Values.OrderBy(v => v.Nombre).ToList();
        return Task.FromResult(lista);
    }

    public Task<Vikingo?> ObtenerPorIdAsync(Guid id) =>
        Task.FromResult(_vikingos.GetValueOrDefault(id));

    public Task<bool> ExisteNombreAsync(string nombre, Guid? excluirId = null) =>
        Task.FromResult(_vikingos.Values.Any(v =>
            v.Id != excluirId && string.Equals(v.Nombre, nombre, StringComparison.OrdinalIgnoreCase)));

    public Task AgregarAsync(Vikingo vikingo)
    {
        _vikingos[vikingo.Id] = vikingo;
        return Task.CompletedTask;
    }

    public Task<bool> ActualizarAsync(Vikingo vikingo)
    {
        if (!_vikingos.ContainsKey(vikingo.Id))
        {
            return Task.FromResult(false);
        }

        _vikingos[vikingo.Id] = vikingo;
        return Task.FromResult(true);
    }

    public Task<bool> EliminarAsync(Guid id) =>
        Task.FromResult(_vikingos.TryRemove(id, out _));
}
