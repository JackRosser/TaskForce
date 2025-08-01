using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto.FasiProgetto.MacroFasi;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/progetti/{progettoId:int}/macrofasi")]
public class MacroFasiController(IMacroFaseService svc) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetMacroFaseDto>>> GetAll(int progettoId, CancellationToken ct)
        => Ok(await svc.GetAllAsync(progettoId, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetMacroFaseDto>> GetById(int progettoId, int id, CancellationToken ct)
    {
        var item = await svc.GetByIdAsync(progettoId, id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<GetMacroFaseDto>> Create(int progettoId, [FromBody] CreateMacroFaseDto dto, CancellationToken ct)
    {
        var created = await svc.CreateAsync(progettoId, dto, ct);
        return CreatedAtAction(nameof(GetById), new { progettoId, id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int progettoId, int id, [FromBody] UpdateMacroFaseDto dto, CancellationToken ct)
    {
        if (id != dto.Id || progettoId != dto.ProgettoId)
            return BadRequest("Id path/body o ProgettoId non coincidono.");

        var ok = await svc.UpdateAsync(dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int progettoId, int id, CancellationToken ct)
    {
        var ok = await svc.DeleteAsync(progettoId, id, ct);
        return ok ? NoContent() : NotFound();
    }

    // Tutte le macro fasi e le relative fasi
    [HttpGet("with-fasi")]
    public async Task<ActionResult<IEnumerable<MacroFaseWithFasiDto>>> GetAllWithFasi(int progettoId, CancellationToken ct)
        => Ok(await svc.GetAllWithFasiAsync(progettoId, ct));
}
