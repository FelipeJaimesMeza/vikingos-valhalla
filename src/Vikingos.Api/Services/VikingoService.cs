using Vikingos.Api.Domain;
using Vikingos.Api.Repositories;
using Vikingos.Shared;

namespace Vikingos.Api.Services;

/// <summary>
/// Orquesta el CRUD: valida reglas de negocio, usa el repositorio para guardar
/// y el calculador para obtener puntos y categoría.
/// </summary>
public class VikingoService(IVikingoRepository repository, IValhallaCalculator calculator) : IVikingoService
{
    public async Task<IReadOnlyList<VikingoResponse>> ObtenerTodosAsync()
    {
        var vikingos = await repository.ObtenerTodosAsync();
        return vikingos.Select(ToResponse).ToList();
    }

    public async Task<VikingoResponse?> ObtenerPorIdAsync(Guid id)
    {
        var vikingo = await repository.ObtenerPorIdAsync(id);
        return vikingo is null ? null : ToResponse(vikingo);
    }

    public async Task<VikingoResponse> CrearAsync(VikingoRequest request)
    {
        var nombre = request.Nombre.Trim();
        await ValidarNombreUnicoAsync(nombre);

        var vikingo = new Vikingo { Nombre = nombre };
        AplicarCambios(vikingo, request);

        await repository.AgregarAsync(vikingo);
        return ToResponse(vikingo);
    }

    public async Task<VikingoResponse?> ActualizarAsync(Guid id, VikingoRequest request)
    {
        var vikingo = await repository.ObtenerPorIdAsync(id);
        if (vikingo is null)
        {
            return null;
        }

        await ValidarNombreUnicoAsync(request.Nombre.Trim(), id);

        AplicarCambios(vikingo, request);
        await repository.ActualizarAsync(vikingo);
        return ToResponse(vikingo);
    }

    public Task<bool> EliminarAsync(Guid id) => repository.EliminarAsync(id);

    private async Task ValidarNombreUnicoAsync(string nombre, Guid? excluirId = null)
    {
        if (await repository.ExisteNombreAsync(nombre, excluirId))
        {
            throw new ConflictoException($"Ya existe un vikingo llamado '{nombre}'.");
        }
    }

    private static void AplicarCambios(Vikingo vikingo, VikingoRequest request)
    {
        // Los "!" son seguros: [ApiController] ya rechazó con 400 los requests sin estos campos.
        vikingo.Nombre = request.Nombre.Trim();
        vikingo.BatallasGanadas = request.BatallasGanadas;
        vikingo.ArmaFavorita = request.ArmaFavorita!.Value;
        vikingo.NivelHonor = request.NivelHonor!.Value;
        vikingo.CausaMuerte = request.CausaMuerte!.Value;
    }

    private VikingoResponse ToResponse(Vikingo vikingo)
    {
        var puntos = calculator.CalcularPuntos(vikingo);
        var aprobado = calculator.EstaAprobado(puntos);

        return new VikingoResponse(
            vikingo.Id,
            vikingo.Nombre,
            vikingo.BatallasGanadas,
            vikingo.ArmaFavorita,
            vikingo.NivelHonor,
            vikingo.CausaMuerte,
            puntos,
            calculator.Clasificar(vikingo),
            aprobado,
            aprobado ? ValhallaCalculator.MensajeAprobado : ValhallaCalculator.MensajeRechazado);
    }
}
