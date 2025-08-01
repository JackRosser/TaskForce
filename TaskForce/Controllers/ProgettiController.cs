using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.Progetto;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgettiController(IProgettoService svc) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetProgettoDto>>> GetAll(CancellationToken ct)
        => Ok(await svc.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetProgettoDto>> GetById(int id, CancellationToken ct)
    {
        var item = await svc.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<GetProgettoDto>> Create([FromBody] CreaProgettoDto dto, CancellationToken ct)
    {
        var created = await svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProgettoDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id path/body non coincidono.");
        var ok = await svc.UpdateAsync(dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
