using System.Text.Json.Serialization;

namespace Vikingos.Shared;

// Los enums viajan como texto en el JSON ("Hacha" en vez de 0) para que la API sea legible.

[JsonConverter(typeof(JsonStringEnumConverter<ArmaFavorita>))]
public enum ArmaFavorita
{
    Hacha,
    Espada,
    Martillo,
    Lanza,
    Arco
}

[JsonConverter(typeof(JsonStringEnumConverter<NivelHonor>))]
public enum NivelHonor
{
    Bajo,
    Medio,
    Alto
}

[JsonConverter(typeof(JsonStringEnumConverter<CausaMuerte>))]
public enum CausaMuerte
{
    BatallaCampoAbierto,
    DueloUnoContraUno,
    ProtegiendoSuAldea
}

[JsonConverter(typeof(JsonStringEnumConverter<CategoriaGuerrero>))]
public enum CategoriaGuerrero
{
    GuerreroDeThor,
    GuerreroDeFreyja,
    GuerreroDeOdin
}
