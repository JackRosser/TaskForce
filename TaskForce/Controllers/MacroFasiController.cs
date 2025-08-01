using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto.FasiProgetto.MacroFasi;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/v1/macrofasi")]
[Tags("MacroFasi")]
public class MacroFasiController(IMacroFaseService svc) : ControllerBase
{
    [HttpGet("with-fasi")]
    [EndpointName("GetAllMacroFasiAndFasi")]
    [EndpointSummary("Ottiene tutte le macro fasi e tutte le fasi a esse associate")]
    [ProducesResponseType(typeof(IEnumerable<MacroFaseWithFasiDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MacroFaseWithFasiDto>>> GetAllWithFasi(int progettoId, CancellationToken ct)
        => Ok(await svc.GetAllWithFasiAsync(progettoId, ct));


    [HttpGet]
    [EndpointName("GetAllMacroFasi")]
    [EndpointSummary("Ottiene tutte le macro fasi")]
    [ProducesResponseType(typeof(IEnumerable<GetMacroFaseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetMacroFaseDto>>> GetAll(int progettoId, CancellationToken ct)
        => Ok(await svc.GetAllAsync(progettoId, ct));

    [HttpGet("{id}")]
    [EndpointName("GetMacroFaseById")]
    [EndpointSummary("Ottiene una singola macro fase")]
    [ProducesResponseType(typeof(GetMacroFaseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetMacroFaseDto>> GetById(int progettoId, int id, CancellationToken ct)
    {
        var item = await svc.GetByIdAsync(progettoId, id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [EndpointName("CreateMacroFase")]
    [EndpointSummary("Crea una nuova macro fase")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<GetMacroFaseDto>> Create(int progettoId, [FromBody] CreateMacroFaseDto dto, CancellationToken ct)
    {
        var created = await svc.CreateAsync(progettoId, dto, ct);
        return CreatedAtAction(nameof(GetById), new { progettoId, id = created.Id }, created);
    }

    [HttpPut("{id}/macrofase")]
    [EndpointName("UpdateMacroFase")]
    [EndpointSummary("Modifica una macro fase")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int progettoId, int id, [FromBody] UpdateMacroFaseDto dto, CancellationToken ct)
    {
        if (id != dto.Id || progettoId != dto.ProgettoId)
            return BadRequest("Id path/body o ProgettoId non coincidono.");

        var ok = await svc.UpdateAsync(dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [EndpointName("DeleteMacroFase")]
    [EndpointSummary("Elimina una macro fase")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int progettoId, int id, CancellationToken ct)
    {
        var ok = await svc.DeleteAsync(progettoId, id, ct);
        return ok ? NoContent() : NotFound();
    }


}
