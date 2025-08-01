using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.User;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/v1/users")]
[Tags("Users")]
public class UsersController(IUserService svc) : ControllerBase
{
    [HttpPost]
    [EndpointName("CreateNewUser")]
    [EndpointSummary("Crea un nuovo utente")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<GetUserDto>> Create([FromBody] CreateUserDto dto, CancellationToken ct)
    {
        var created = await svc.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    [EndpointName("GetUserById")]
    [EndpointSummary("Ottiene uno user")]
    [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetUserDto>> GetById(int id, CancellationToken ct)
    {
        var user = await svc.GetByIdAsync(id, ct);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet]
    [EndpointName("GetAllUsers")]
    [EndpointSummary("Ottiene tutti gli utenti registrati")]
    [ProducesResponseType(typeof(IEnumerable<GetUserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll(CancellationToken ct)
    => Ok(await svc.GetAllAsync(ct)); // ← tipo aggiornato


    [HttpPut("{id}/user")]
    [EndpointName("UpdateUser")]
    [EndpointSummary("Modifica un utente")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id path/body non coincidono.");
        var ok = await svc.UpdateAsync(dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [EndpointName("DeleteUser")]
    [EndpointSummary("Elimina un utente")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
