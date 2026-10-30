using Vikingos.Api.Domain;
using Vikingos.Shared;

namespace Vikingos.Tests;

public class ValhallaCalculatorTests
{
    private readonly ValhallaCalculator _calculator = new();

    private static Vikingo Crear(int batallas, ArmaFavorita arma, NivelHonor honor, CausaMuerte causa) => new()
    {
        Nombre = "Prueba",
        BatallasGanadas = batallas,
        ArmaFavorita = arma,
        NivelHonor = honor,
        CausaMuerte = causa
    };

    [Theory]
    // batallas*10 + honor(Bajo 0 / Medio 50 / Alto 100) + muerte(Aldea 60 / Campo 50 / Duelo 40)
    [InlineData(0, NivelHonor.Bajo, CausaMuerte.DueloUnoContraUno, 40)]
    [InlineData(20, NivelHonor.Medio, CausaMuerte.BatallaCampoAbierto, 300)]
    [InlineData(120, NivelHonor.Alto, CausaMuerte.ProtegiendoSuAldea, 1360)]
    public void CalcularPuntos_AplicaLaFormula(int batallas, NivelHonor honor, CausaMuerte causa, int esperado)
    {
        var puntos = _calculator.CalcularPuntos(Crear(batallas, ArmaFavorita.Espada, honor, causa));

        Assert.Equal(esperado, puntos);
    }

    [Theory]
    [InlineData(ArmaFavorita.Martillo, CategoriaGuerrero.GuerreroDeThor)]
    [InlineData(ArmaFavorita.Hacha, CategoriaGuerrero.GuerreroDeThor)]
    [InlineData(ArmaFavorita.Espada, CategoriaGuerrero.GuerreroDeFreyja)]
    [InlineData(ArmaFavorita.Lanza, CategoriaGuerrero.GuerreroDeOdin)]
    [InlineData(ArmaFavorita.Arco, CategoriaGuerrero.GuerreroDeOdin)]
    public void Clasificar_SegunArma(ArmaFavorita arma, CategoriaGuerrero esperada)
    {
        var categoria = _calculator.Clasificar(Crear(10, arma, NivelHonor.Medio, CausaMuerte.DueloUnoContraUno));

        Assert.Equal(esperada, categoria);
    }

    [Theory]
    [InlineData(299, false)]
    [InlineData(300, true)]
    [InlineData(301, true)]
    public void EstaAprobado_UsaElUmbral(int puntos, bool esperado) =>
        Assert.Equal(esperado, _calculator.EstaAprobado(puntos));
}
