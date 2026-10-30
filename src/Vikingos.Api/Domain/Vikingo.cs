using Vikingos.Shared;

namespace Vikingos.Api.Domain;

/// <summary>Entidad que se guarda en memoria.</summary>
public class Vikingo
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Nombre { get; set; }
    public int BatallasGanadas { get; set; }
    public ArmaFavorita ArmaFavorita { get; set; }
    public NivelHonor NivelHonor { get; set; }
    public CausaMuerte CausaMuerte { get; set; }
}
