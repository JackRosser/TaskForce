using TaskForce.Dto.Progetto.PresaInCarico;

namespace TaskForce.Services;

public interface IPresaInCaricoService
{
    Task<IEnumerable<PresaInCaricoDto>> GetByFaseAsync(int faseProgettoId, CancellationToken ct = default);
    Task<PresaInCaricoDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PresaInCaricoDto> AssegnaAsync(int faseProgettoId, int userId, CancellationToken ct = default);
}
