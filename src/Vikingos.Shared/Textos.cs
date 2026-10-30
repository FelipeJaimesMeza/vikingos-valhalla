namespace Vikingos.Shared;

/// <summary>Textos amigables para mostrar los enums en pantalla.</summary>
public static class Textos
{
    public static string ToTexto(this ArmaFavorita arma) => arma switch
    {
        ArmaFavorita.Hacha => "Hacha",
        ArmaFavorita.Espada => "Espada",
        ArmaFavorita.Martillo => "Martillo",
        ArmaFavorita.Lanza => "Lanza",
        ArmaFavorita.Arco => "Arco",
        _ => arma.ToString()
    };

    public static string ToTexto(this NivelHonor honor) => honor switch
    {
        NivelHonor.Bajo => "Bajo",
        NivelHonor.Medio => "Medio",
        NivelHonor.Alto => "Alto",
        _ => honor.ToString()
    };

    public static string ToTexto(this CausaMuerte causa) => causa switch
    {
        CausaMuerte.BatallaCampoAbierto => "Batalla en campo abierto",
        CausaMuerte.DueloUnoContraUno => "Duelo uno contra uno",
        CausaMuerte.ProtegiendoSuAldea => "Protegiendo su aldea",
        _ => causa.ToString()
    };

    public static string ToTexto(this CategoriaGuerrero categoria) => categoria switch
    {
        CategoriaGuerrero.GuerreroDeThor => "Guerrero de Thor",
        CategoriaGuerrero.GuerreroDeFreyja => "Guerrero de Freyja",
        CategoriaGuerrero.GuerreroDeOdin => "Guerrero de Odín",
        _ => categoria.ToString()
    };
}
