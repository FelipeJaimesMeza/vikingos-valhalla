using Vikingos.Shared;

namespace Vikingos.Api.Domain;

/// <summary>Reglas de negocio del Valhalla: puntos, categoría y aprobación.</summary>
public interface IValhallaCalculator
{
    int CalcularPuntos(Vikingo vikingo);
    CategoriaGuerrero Clasificar(Vikingo vikingo);
    bool EstaAprobado(int valhallaPoints);
}
