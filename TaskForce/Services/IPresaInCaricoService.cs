using TaskForce.Dto.Progetto.PresaInCarico;

public interface IPresaInCaricoService
{
    Task<IEnumerable<PresaInCaricoRequest>> GetByFaseAsync(int faseProgettoId, CancellationToken ct = default);
    Task<PresaInCaricoRequest?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PresaInCaricoRequest> AssegnaAsync(int faseProgettoId, int userId, CancellationToken ct = default);
    Task PausaAsync(int id, CancellationToken ct = default);
    Task RiprendiAsync(int id, CancellationToken ct = default);
    Task ConcludiAsync(int id, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
