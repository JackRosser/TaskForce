using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto.FasiProgetto;
using TaskForce.Dto.Progetto.PresaInCarico;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
public class PauseController(IPausaService svc) : ControllerBase
{
    // GET: api/prese-in-carico/{presaInCaricoId}/pause
    [HttpGet("api/prese-in-carico/{presaInCaricoId:int}/pause")]
    public async Task<ActionResult<IEnumerable<PausaDto>>> GetByPresa(int presaInCaricoId, CancellationToken ct)
        => Ok(await svc.GetByPresaAsync(presaInCaricoId, ct));

    // START: api/prese-in-carico/{presaInCaricoId}/pause  Body: { "userId": 123 }

    [HttpPost("api/prese-in-carico/{presaInCaricoId:int}/pause")]
    public async Task<ActionResult<PausaDto>> Start(int presaInCaricoId, [FromBody] PausaRequest req, CancellationToken ct)
    {
        if (req.UserId <= 0) return BadRequest("UserId non valido.");
        try
        {
            var created = await svc.StartAsync(presaInCaricoId, req.UserId, ct);
            return CreatedAtAction(nameof(GetByPresa), new { presaInCaricoId }, created);
        }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (InvalidOperationException e) { return Conflict(e.Message); }
    }

    // STOP: api/pause/{pausaId}/stop  Body: { "userId": 123 } (facoltativo)

    [HttpPost("api/pause/{pausaId:int}/stop")]
    public async Task<IActionResult> Stop(int pausaId, [FromBody] PausaRequest req, CancellationToken ct)
    {
        var ok = await svc.StopAsync(pausaId, req.UserId, ct);
        return ok ? NoContent() : NotFound();
    }
}
