using Vikingos.Shared;

namespace Vikingos.Api.Domain;

public class ValhallaCalculator : IValhallaCalculator
{
    public const int PuntosPorBatalla = 10;
    public const int UmbralValhalla = 300;
    public const string MensajeAprobado = "¡Listo para el banquete en Valhalla!";
    public const string MensajeRechazado = "Aún no tiene el honor suficiente para entrar al Valhalla.";

    /// <summary>
    /// Valhalla Points = batallas * 10 + bono por nivel de honor + bono por causa de muerte.
    /// </summary>
    public int CalcularPuntos(Vikingo vikingo)
    {
        var bonoHonor = vikingo.NivelHonor switch
        {
            NivelHonor.Alto => 100,
            NivelHonor.Medio => 50,
            _ => 0
        };

        var bonoMuerte = vikingo.CausaMuerte switch
        {
            CausaMuerte.ProtegiendoSuAldea => 60,
            CausaMuerte.BatallaCampoAbierto => 50,
            CausaMuerte.DueloUnoContraUno => 40,
            _ => 0
        };

        return vikingo.BatallasGanadas * PuntosPorBatalla + bonoHonor + bonoMuerte;
    }

    /// <summary>
    /// Thor = fuerza (martillo, hacha). Freyja = táctica cuerpo a cuerpo (espada).
    /// Odín = estrategas que pelean a distancia (lanza, arco).
    /// </summary>
    public CategoriaGuerrero Clasificar(Vikingo vikingo) => vikingo.ArmaFavorita switch
    {
        ArmaFavorita.Martillo or ArmaFavorita.Hacha => CategoriaGuerrero.GuerreroDeThor,
        ArmaFavorita.Espada => CategoriaGuerrero.GuerreroDeFreyja,
        _ => CategoriaGuerrero.GuerreroDeOdin
    };

    public bool EstaAprobado(int valhallaPoints) => valhallaPoints >= UmbralValhalla;
}
