using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto.PresaInCarico;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/v1/preseincarico")]
[Tags("PreseInCarico")]
public class PreseInCaricoController(IPresaInCaricoService svc) : ControllerBase
{
    [HttpGet("fase/{id}/presaincarico")]
    [EndpointName("GetAllPreseInCarico")]
    [EndpointSummary("Ottiene tutte le prese in carico di una fase")]
    [ProducesResponseType(typeof(IEnumerable<PresaInCaricoRequest>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PresaInCaricoRequest>>> GetAll(int id, CancellationToken ct)
        => Ok(await svc.GetByFaseAsync(id, ct));

    [HttpGet("presaincarico/{id}")]
    [EndpointName("GetSinglePresaInCarico")]
    [EndpointSummary("Ottiene una singola presa in carico")]
    [ProducesResponseType(typeof(PresaInCaricoRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PresaInCaricoRequest>> GetById(int id, CancellationToken ct)
    {
        var item = await svc.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("fase/{id}/presaincarico")]
    [EndpointName("InitPresaInCarico")]
    [EndpointSummary("Un utente inizializza una presa in carico")]
    [ProducesResponseType(typeof(PresaInCaricoRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PresaInCaricoRequest>> Assegna(int id, [FromBody] PresaInCaricoRequest dto, CancellationToken ct)
    {
        if (dto is null || dto.UserId <= 0)
            return BadRequest("UserId mancante o non valido.");

        var created = await svc.AssegnaAsync(id, dto.UserId, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id}/pausa")]
    [EndpointName("PausaPresaInCarico")]
    [EndpointSummary("L'utente mette in pausa la propria presa in carico")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Pausa(int id, CancellationToken ct)
    {
        await svc.PausaAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id}/riprendi")]
    [EndpointName("RiprendiPresaInCarico")]
    [EndpointSummary("L'utente riprende il lavoro")]
    public async Task<IActionResult> Riprendi(int id, CancellationToken ct)
    {
        await svc.RiprendiAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id}/concludi")]
    [EndpointName("ConcludiPresaInCarico")]
    [EndpointSummary("L'utente ha terminato il lavoro")]
    public async Task<IActionResult> Concludi(int id, CancellationToken ct)
    {
        await svc.ConcludiAsync(id, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [EndpointName("EliminaPresaInCarico")]
    [EndpointSummary("Elimina una presa in carico")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await svc.DeleteAsync(id, ct);
        return NoContent();
    }

}
