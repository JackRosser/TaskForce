using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto.FasiProgetto;

[ApiController]
[Route("api/v1/fasi")]
[Tags("Fasi")]
public class FasiProgettoController(IFaseProgettoService svc) : ControllerBase
{
    [HttpGet]
    [EndpointName("GetAllFasi")]
    [EndpointSummary("Ottiene tutte le fasi di una macro fase")]
    [ProducesResponseType(typeof(IEnumerable<GetFaseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetFaseDto>>> GetAll(int macroFaseId, CancellationToken ct)
        => Ok(await svc.GetAllAsync(macroFaseId, ct));

    [HttpGet("{id}")]
    [EndpointName("GetFaseById")]
    [EndpointSummary("Ottiene una singola fase")]
    [ProducesResponseType(typeof(GetFaseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetFaseDto>> GetById(int macroFaseId, int id, CancellationToken ct)
    {
        var item = await svc.GetByIdAsync(macroFaseId, id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [EndpointName("CreateFase")]
    [EndpointSummary("Crea una nuova fase all'interno di una macro fase")]
    [ProducesResponseType(typeof(GetFaseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<GetFaseDto>> Create(int macroFaseId, [FromBody] CreateFaseDto dto, CancellationToken ct)
    {
        var created = await svc.CreateAsync(macroFaseId, dto, ct);
        return CreatedAtAction(nameof(GetById), new { macroFaseId, id = created.Id }, created);
    }

    [HttpPut("{id}/fase")]
    [EndpointName("UpdateFase")]
    [EndpointSummary("Modifica una singola fase")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int macroFaseId, int id, [FromBody] UpdateFaseProgettoDto dto, CancellationToken ct)
    {
        if (id != dto.Id || macroFaseId != dto.MacroFaseId)
            return BadRequest("Id path/body o MacroFaseId non coincidono.");

        var ok = await svc.UpdateAsync(dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [EndpointName("DeleteFase")]
    [EndpointSummary("Elimina una singola fase")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int macroFaseId, int id, CancellationToken ct)
    {
        var ok = await svc.DeleteAsync(macroFaseId, id, ct);
        return ok ? NoContent() : NotFound();
    }
}
