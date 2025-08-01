using TaskForce.Dto.User;

namespace TaskForce.Services;

public interface IUsersFasiService
{
    Task<IEnumerable<UserFaseCorrenteDto>> GetFasiCorrentiAsync(CancellationToken ct = default);
    Task<IEnumerable<UserFaseDto>> GetFasiCompletateByUserAsync(int userId, CancellationToken ct = default);
    Task<UserFasiOverviewDto?> GetOverviewByUserAsync(int userId, CancellationToken ct = default);
    Task<bool> CompletaFaseAsync(int faseProgettoId, CancellationToken ct = default);

}
