using TaskForce.Dto.Progetto.FasiProgetto.MacroFasi;

namespace TaskForce.Services;

public interface IMacroFaseService
{
    Task<GetMacroFaseDto> CreateAsync(int progettoId, CreateMacroFaseDto dto, CancellationToken ct = default);
    Task<GetMacroFaseDto?> GetByIdAsync(int progettoId, int id, CancellationToken ct = default);
    Task<IEnumerable<GetMacroFaseDto>> GetAllAsync(int progettoId, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateMacroFaseDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);

}
