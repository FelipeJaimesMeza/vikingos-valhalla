using Microsoft.AspNetCore.Mvc;
using Vikingos.Api.Services;
using Vikingos.Shared;

namespace Vikingos.Api.Controllers;

/// <summary>
/// Endpoints REST del CRUD. El controlador solo traduce HTTP ⇄ servicio; no tiene lógica de negocio.
/// [ApiController] valida automáticamente el body y responde 400 si algo no cumple.
/// </summary>
[ApiController]
[Route("api/vikingos")]
[Produces("application/json")]
public class VikingosController(IVikingoService service) : ControllerBase
{
    // GET api/vikingos
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<VikingoResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<VikingoResponse>>> ObtenerTodos() =>
        Ok(await service.ObtenerTodosAsync());

    // GET api/vikingos/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType<VikingoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VikingoResponse>> ObtenerPorId(Guid id)
    {
        var vikingo = await service.ObtenerPorIdAsync(id);
        return vikingo is null ? NoEncontrado(id) : Ok(vikingo);
    }

    // POST api/vikingos
    [HttpPost]
    [ProducesResponseType<VikingoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<VikingoResponse>> Crear(VikingoRequest request)
    {
        var creado = await service.CrearAsync(request);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    // PUT api/vikingos/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType<VikingoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<VikingoResponse>> Actualizar(Guid id, VikingoRequest request)
    {
        var actualizado = await service.ActualizarAsync(id, request);
        return actualizado is null ? NoEncontrado(id) : Ok(actualizado);
    }

    // DELETE api/vikingos/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(Guid id) =>
        await service.EliminarAsync(id) ? NoContent() : NoEncontrado(id);

    private ObjectResult NoEncontrado(Guid id) =>
        Problem(statusCode: StatusCodes.Status404NotFound,
                title: "Vikingo no encontrado",
                detail: $"No existe un vikingo con id {id}.");
}
