using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/v1/progetti")]
[Tags("Progetti")]
public class ProgettiController(IProgettoService svc) : ControllerBase
{
    [HttpGet]
    [EndpointName("GetAllProgetti")]
    [EndpointSummary("Ottiene tutti i progetti in corso")]
    [ProducesResponseType(typeof(IEnumerable<GetProgettoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetProgettoDto>>> GetAll(CancellationToken ct)
        => Ok(await svc.GetAllAsync(ct));

    [HttpGet("{id}")]
    [EndpointName("GetSingleProgetto")]
    [EndpointSummary("Ottiene un singolo progetto")]
    [ProducesResponseType(typeof(GetProgettoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetProgettoDto>> GetById(int id, CancellationToken ct)
    {
        var item = await svc.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [EndpointName("CreaProgetto")]
    [EndpointSummary("Crea un nuovo progetto")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<GetProgettoDto>> Create([FromBody] CreaProgettoDto dto, CancellationToken ct)
    {
        var created = await svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("progetto/{id}")]
    [EndpointName("UpdateProgetto")]
    [EndpointSummary("Modifica un progetto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProgettoDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id path/body non coincidono.");
        var ok = await svc.UpdateAsync(dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [EndpointName("DeleteProgetto")]
    [EndpointSummary("Elimina un progetto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("allinformations")]
    [EndpointName("GetProgettiWithFasiInfo")]
    [EndpointSummary("Ottiene tutti i progetti con macrofasi, fasi e dettagli delle prese in carico")]
    [ProducesResponseType(typeof(IEnumerable<GetProgettoWithFasiRequest>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetProgettoWithFasiRequest>>> GetAllWithInfo(CancellationToken ct)
        => Ok(await svc.GetAllWithInfoAsync(ct));

}
