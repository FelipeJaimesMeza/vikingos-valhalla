using System.ComponentModel.DataAnnotations;

namespace Vikingos.Shared;

/// <summary>
/// Datos que envía el formulario para crear o editar un vikingo.
/// Las mismas reglas de validación se usan en Blazor (frontend) y en la API (backend).
/// </summary>
public class VikingoRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0, 1000, ErrorMessage = "Las batallas ganadas deben estar entre 0 y 1000.")]
    public int BatallasGanadas { get; set; }

    // Nullable + [Required]: obliga al usuario a escoger una opción en vez de tomar el primer valor por defecto.
    [Required(ErrorMessage = "Seleccione un arma favorita.")]
    public ArmaFavorita? ArmaFavorita { get; set; }

    [Required(ErrorMessage = "Seleccione un nivel de honor.")]
    public NivelHonor? NivelHonor { get; set; }

    [Required(ErrorMessage = "Seleccione la causa de muerte gloriosa.")]
    public CausaMuerte? CausaMuerte { get; set; }
}
