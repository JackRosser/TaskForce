using Microsoft.AspNetCore.Mvc;
using TaskForce.Dto.User;
using TaskForce.Services;

namespace TaskForce.Controllers;

[ApiController]
[Route("api/users/fasi")]
public class UsersFasiController(IUsersFasiService svc) : ControllerBase
{
    // Correnti per tutti gli utenti
    // GET: api/users/fasi/correnti
    [HttpGet("correnti")]
    public async Task<ActionResult<IEnumerable<UserFaseCorrenteDto>>> GetFasiCorrenti(CancellationToken ct)
        => Ok(await svc.GetFasiCorrentiAsync(ct));

    // Completate per uno user
    // GET: api/users/{userId}/fasi/completate
    [HttpGet("~/api/users/{userId:int}/fasi/completate")]
    public async Task<ActionResult<IEnumerable<UserFaseDto>>> GetFasiCompletate(int userId, CancellationToken ct)
        => Ok(await svc.GetFasiCompletateByUserAsync(userId, ct));

    // Overview singolo
    // GET: api/users/{userId}/fasi/overview
    [HttpGet("~/api/users/{userId:int}/fasi/overview")]
    public async Task<ActionResult<UserFasiOverviewDto>> GetOverview(int userId, CancellationToken ct)
    {
        var overview = await svc.GetOverviewByUserAsync(userId, ct);
        return overview is null ? NotFound() : Ok(overview);
    }
}
