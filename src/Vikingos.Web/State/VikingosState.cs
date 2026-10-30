using Vikingos.Shared;
using Vikingos.Web.Services;

namespace Vikingos.Web.State;

/// <summary>
/// State Management: única fuente de verdad del frontend.
/// Guarda la lista, el vikingo en edición, si está cargando y el último error/éxito.
/// Los componentes leen de aquí y se suscriben a <see cref="OnChange"/> para redibujarse.
/// </summary>
public class VikingosState(IVikingoApiClient api)
{
    private List<VikingoResponse> _vikingos = [];

    public IReadOnlyList<VikingoResponse> Vikingos => _vikingos;
    public VikingoResponse? EnEdicion { get; private set; }
    public bool Cargando { get; private set; }
    public string? Error { get; private set; }
    public string? Exito { get; private set; }

    public event Action? OnChange;

    public async Task CargarAsync()
    {
        await EjecutarAsync(async () =>
        {
            var resultado = await api.ObtenerTodosAsync();
            if (resultado.Exito)
            {
                _vikingos = resultado.Datos ?? [];
            }
            return resultado.Error;
        });
    }

    /// <summary>Crea o actualiza según haya un vikingo en edición. Devuelve true si se guardó.</summary>
    public async Task<bool> GuardarAsync(VikingoRequest request)
    {
        var guardado = false;
        await EjecutarAsync(async () =>
        {
            var editando = EnEdicion;
            var resultado = editando is null
                ? await api.CrearAsync(request)
                : await api.ActualizarAsync(editando.Id, request);

            if (!resultado.Exito)
            {
                return resultado.Error;
            }

            // Actualización dinámica de la lista sin volver a pedirla completa al servidor.
            var vikingo = resultado.Datos!;
            _vikingos = _vikingos.Where(v => v.Id != vikingo.Id).Append(vikingo).OrderBy(v => v.Nombre).ToList();
            EnEdicion = null;
            Exito = editando is null ? $"{vikingo.Nombre} fue registrado." : $"{vikingo.Nombre} fue actualizado.";
            guardado = true;
            return null;
        });
        return guardado;
    }

    public async Task EliminarAsync(VikingoResponse vikingo)
    {
        await EjecutarAsync(async () =>
        {
            var resultado = await api.EliminarAsync(vikingo.Id);
            if (!resultado.Exito)
            {
                return resultado.Error;
            }

            _vikingos = _vikingos.Where(v => v.Id != vikingo.Id).ToList();
            if (EnEdicion?.Id == vikingo.Id)
            {
                EnEdicion = null;
            }
            Exito = $"{vikingo.Nombre} fue eliminado.";
            return null;
        });
    }

    public void Editar(VikingoResponse vikingo)
    {
        EnEdicion = vikingo;
        LimpiarMensajes();
        Notificar();
    }

    public void CancelarEdicion()
    {
        EnEdicion = null;
        Notificar();
    }

    public void LimpiarMensajes()
    {
        Error = null;
        Exito = null;
        Notificar();
    }

    /// <summary>Patrón común: marca "cargando", ejecuta la operación, guarda el error y avisa a la UI.</summary>
    private async Task EjecutarAsync(Func<Task<string?>> operacion)
    {
        Cargando = true;
        Error = null;
        Exito = null;
        Notificar();

        try
        {
            Error = await operacion();
        }
        finally
        {
            Cargando = false;
            Notificar();
        }
    }

    private void Notificar() => OnChange?.Invoke();
}
