using Vikingos.Api.Domain;
using Vikingos.Api.Repositories;
using Vikingos.Shared;

namespace Vikingos.Api.Infrastructure;

/// <summary>Carga unos vikingos de ejemplo para que la lista no arranque vacía.</summary>
public static class DatosIniciales
{
    public static async Task CargarAsync(IVikingoRepository repository)
    {
        Vikingo[] vikingos =
        [
            new() { Nombre = "Ragnar", BatallasGanadas = 120, ArmaFavorita = ArmaFavorita.Hacha, NivelHonor = NivelHonor.Alto, CausaMuerte = CausaMuerte.BatallaCampoAbierto },
            new() { Nombre = "Lagertha", BatallasGanadas = 70, ArmaFavorita = ArmaFavorita.Espada, NivelHonor = NivelHonor.Alto, CausaMuerte = CausaMuerte.ProtegiendoSuAldea },
            new() { Nombre = "Floki", BatallasGanadas = 10, ArmaFavorita = ArmaFavorita.Arco, NivelHonor = NivelHonor.Medio, CausaMuerte = CausaMuerte.DueloUnoContraUno }
        ];

        foreach (var vikingo in vikingos)
        {
            await repository.AgregarAsync(vikingo);
        }
    }
}
