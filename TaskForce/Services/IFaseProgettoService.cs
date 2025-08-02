using TaskForce.Dto.Progetto.FasiProgetto;

public interface IFaseProgettoService
{
    Task<GetFaseDto> CreateAsync(int macroFaseId, CreateFaseDto dto, CancellationToken ct = default);
    Task<GetFaseDto?> GetByIdAsync(int macroFaseId, int id, CancellationToken ct = default);
    Task<IEnumerable<GetFaseDto>> GetAllAsync(int macroFaseId, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateFaseProgettoDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
