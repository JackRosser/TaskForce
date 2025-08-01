using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto.PresaInCarico;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/fasi/{faseProgettoId:int}/prese-in-carico")]
public class PreseInCaricoController(IPresaInCaricoService svc) : ControllerBase
{
    // GET: api/fasi/{faseProgettoId}/prese-in-carico
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PresaInCaricoDto>>> GetAll(int faseProgettoId, CancellationToken ct)
        => Ok(await svc.GetByFaseAsync(faseProgettoId, ct));

    // (facoltativo) GET singolo per id assoluto (non annidato alla fase)
    [HttpGet("~/api/prese-in-carico/{id:int}")]
    public async Task<ActionResult<PresaInCaricoDto>> GetById(int id, CancellationToken ct)
    {
        var item = await svc.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: api/fasi/{faseProgettoId}/prese-in-carico
    // Body: PresaInCaricoDto con almeno UserId valorizzato. FaseProgettoId dal path.
    [HttpPost]
    public async Task<ActionResult<PresaInCaricoDto>> Assegna(int faseProgettoId, [FromBody] PresaInCaricoDto dto, CancellationToken ct)
    {
        if (dto is null || dto.UserId <= 0)
            return BadRequest("UserId mancante o non valido.");

        try
        {
            var created = await svc.AssegnaAsync(faseProgettoId, dto.UserId, ct);
            // ritorno 201 su risorsa globale /api/prese-in-carico/{id}
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


}
