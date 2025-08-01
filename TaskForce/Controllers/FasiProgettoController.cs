using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto.FasiProgetto;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/macrofasi/{macroFaseId:int}/fasi")]
public class FasiProgettoController(IFaseProgettoService svc) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetFaseDto>>> GetAll(int macroFaseId, CancellationToken ct)
        => Ok(await svc.GetAllAsync(macroFaseId, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetFaseDto>> GetById(int macroFaseId, int id, CancellationToken ct)
    {
        var item = await svc.GetByIdAsync(macroFaseId, id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<GetFaseDto>> Create(int macroFaseId, [FromBody] CreateFaseDto dto, CancellationToken ct)
    {
        var created = await svc.CreateAsync(macroFaseId, dto, ct);
        return CreatedAtAction(nameof(GetById), new { macroFaseId, id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int macroFaseId, int id, [FromBody] UpdateFaseProgettoDto dto, CancellationToken ct)
    {
        if (id != dto.Id || macroFaseId != dto.MacroFaseId)
            return BadRequest("Id path/body o MacroFaseId non coincidono.");

        var ok = await svc.UpdateAsync(dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int macroFaseId, int id, CancellationToken ct)
    {
        var ok = await svc.DeleteAsync(macroFaseId, id, ct);
        return ok ? NoContent() : NotFound();
    }
}
