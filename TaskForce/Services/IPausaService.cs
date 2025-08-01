using TaskForce.Dto.Progetto.PresaInCarico;

namespace TaskForce.Services;

public interface IPausaService
{
    Task<PausaDto> StartAsync(int presaInCaricoId, int userId, CancellationToken ct = default);
    Task<bool> StopAsync(int pausaId, int userId, CancellationToken ct = default);
    Task<IEnumerable<PausaDto>> GetByPresaAsync(int presaInCaricoId, CancellationToken ct = default);
}
