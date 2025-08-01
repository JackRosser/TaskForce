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

    [HttpDelete("presaincarico/{id}")]
    [EndpointName("AnnullaPresaInCarico")]
    [EndpointSummary("Un utente annulla la propria presa in carico")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Elimina(int id, CancellationToken ct)
    {
        var ok = await svc.EliminaAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("presaincarico/{id}/pausa")]
    [EndpointName("PausaPresaInCarico")]
    [EndpointSummary("L'utente mette in pausa la propria presa in carico")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MettiInPausa(int id, CancellationToken ct)
    {
        var ok = await svc.MettiInPausaAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("presaincarico/{id}/termina")]
    [EndpointName("TerminaPresaInCarico")]
    [EndpointSummary("L'utente termina il lavoro associato alla presa in carico")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Termina(int id, CancellationToken ct)
    {
        var ok = await svc.TerminaAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
