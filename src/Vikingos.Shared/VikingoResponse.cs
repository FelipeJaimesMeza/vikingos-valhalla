namespace Vikingos.Shared;

/// <summary>Lo que la API devuelve de un vikingo, ya con sus puntos y categoría calculados.</summary>
public record VikingoResponse(
    Guid Id,
    string Nombre,
    int BatallasGanadas,
    ArmaFavorita ArmaFavorita,
    NivelHonor NivelHonor,
    CausaMuerte CausaMuerte,
    int ValhallaPoints,
    CategoriaGuerrero Categoria,
    bool AprobadoParaValhalla,
    string Mensaje)
{
    /// <summary>Convierte la respuesta en un request para precargar el formulario de edición.</summary>
    public VikingoRequest ToRequest() => new()
    {
        Nombre = Nombre,
        BatallasGanadas = BatallasGanadas,
        ArmaFavorita = ArmaFavorita,
        NivelHonor = NivelHonor,
        CausaMuerte = CausaMuerte
    };
}
