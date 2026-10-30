using Vikingos.Api.Domain;
using Vikingos.Api.Repositories;
using Vikingos.Api.Services;
using Vikingos.Shared;

namespace Vikingos.Tests;

public class VikingoServiceTests
{
    private readonly VikingoService _service = new(new InMemoryVikingoRepository(), new ValhallaCalculator());

    private static VikingoRequest Request(string nombre = "Bjorn", int batallas = 85) => new()
    {
        Nombre = nombre,
        BatallasGanadas = batallas,
        ArmaFavorita = ArmaFavorita.Martillo,
        NivelHonor = NivelHonor.Alto,
        CausaMuerte = CausaMuerte.BatallaCampoAbierto
    };

    [Fact]
    public async Task Crear_CalculaPuntosCategoriaYMensaje()
    {
        var creado = await _service.CrearAsync(Request());

        Assert.Equal(85 * 10 + 100 + 50, creado.ValhallaPoints);
        Assert.Equal(CategoriaGuerrero.GuerreroDeThor, creado.Categoria);
        Assert.True(creado.AprobadoParaValhalla);
        Assert.Equal("¡Listo para el banquete en Valhalla!", creado.Mensaje);
    }

    [Fact]
    public async Task Crear_ConNombreRepetido_LanzaConflicto()
    {
        await _service.CrearAsync(Request("Ragnar"));

        await Assert.ThrowsAsync<ConflictoException>(() => _service.CrearAsync(Request("ragnar")));
    }

    [Fact]
    public async Task Actualizar_ModificaElVikingo()
    {
        var creado = await _service.CrearAsync(Request());

        var actualizado = await _service.ActualizarAsync(creado.Id, Request("Bjorn Ironside", 5));

        Assert.NotNull(actualizado);
        Assert.Equal("Bjorn Ironside", actualizado.Nombre);
        Assert.False(actualizado.AprobadoParaValhalla);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_DevuelveNull() =>
        Assert.Null(await _service.ActualizarAsync(Guid.NewGuid(), Request()));

    [Fact]
    public async Task Eliminar_QuitaElVikingo()
    {
        var creado = await _service.CrearAsync(Request());

        Assert.True(await _service.EliminarAsync(creado.Id));
        Assert.Null(await _service.ObtenerPorIdAsync(creado.Id));
        Assert.False(await _service.EliminarAsync(creado.Id));
    }
}
