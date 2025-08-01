using TaskForce.Dto.Progetto.PresaInCarico;

public interface IPresaInCaricoService
{
    Task<IEnumerable<PresaInCaricoRequest>> GetByFaseAsync(int faseProgettoId, CancellationToken ct = default);
    Task<PresaInCaricoRequest?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PresaInCaricoRequest> AssegnaAsync(int faseProgettoId, int userId, CancellationToken ct = default);
    Task<bool> EliminaAsync(int id, CancellationToken ct = default);
    Task<bool> MettiInPausaAsync(int id, CancellationToken ct = default);
    Task<bool> TerminaAsync(int id, CancellationToken ct = default);
}
