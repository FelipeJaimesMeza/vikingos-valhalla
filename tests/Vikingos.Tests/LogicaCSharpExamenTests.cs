namespace Vikingos.Tests;

/// <summary>
/// Código copiado tal cual de la pregunta "Lógica C#" de la prueba,
/// para comprobar ejecutándolo que la tabla de respuestas es correcta.
/// </summary>
public class LogicaCSharpExamenTests
{
    private class Vikingo
    {
        public string Nombre { get; set; } = "";
        public int BatallasGanadas { get; set; }
        public int AniosServicio { get; set; }
        public bool LealtadAGuerreros { get; set; }
        public bool SacrificiosADioses { get; set; }
        public bool HazañaEspecial { get; set; }
    }

    private static int Puntaje(Vikingo v) =>
        (v.BatallasGanadas * 2) + (v.AniosServicio * 5) +
        (v.SacrificiosADioses ? 50 : 0) + (v.HazañaEspecial ? 100 : 0);

    private static string ClasificarVikingo(Vikingo vikingo)
    {
        int puntajeTotal = Puntaje(vikingo);

        if (puntajeTotal > 300 && vikingo.BatallasGanadas > 80)
            return "Campeón del Valhalla";
        else if (puntajeTotal >= 200 && puntajeTotal <= 300 && vikingo.SacrificiosADioses)
            return "Héroe Legendario";
        else if (puntajeTotal >= 100 && puntajeTotal < 200 && vikingo.BatallasGanadas >= 30 && vikingo.LealtadAGuerreros)
            return "Guerrero Valiente";
        else if (vikingo.AniosServicio >= 20 || vikingo.HazañaEspecial)
            return "Veterano Valeroso";
        else
            return "Destino Desconocido";
    }

    public static TheoryData<string, int, int, bool, bool, bool, int, string> Casos => new()
    {
        { "Ragnar", 120, 15, true, true, true, 465, "Campeón del Valhalla" },
        { "Floki", 40, 8, true, false, false, 120, "Guerrero Valiente" },
        { "Bjorn", 85, 16, true, true, true, 400, "Campeón del Valhalla" },
        { "Lagertha", 70, 22, true, false, false, 250, "Veterano Valeroso" }
    };

    [Theory]
    [MemberData(nameof(Casos))]
    public void Clasificacion_CoincideConLaRespuesta(string nombre, int batallas, int anios, bool lealtad,
        bool sacrificios, bool hazaña, int puntosEsperados, string clasificacionEsperada)
    {
        var vikingo = new Vikingo
        {
            Nombre = nombre, BatallasGanadas = batallas, AniosServicio = anios,
            LealtadAGuerreros = lealtad, SacrificiosADioses = sacrificios, HazañaEspecial = hazaña
        };

        Assert.Equal(puntosEsperados, Puntaje(vikingo));
        Assert.Equal(clasificacionEsperada, ClasificarVikingo(vikingo));
    }
}
