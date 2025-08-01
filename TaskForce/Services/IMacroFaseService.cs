using TaskForce.Dto.Progetto.FasiProgetto.MacroFasi;

namespace TaskForce.Services;

public interface IMacroFaseService
{
    Task<GetMacroFaseDto> CreateAsync(int progettoId, CreateMacroFaseDto dto, CancellationToken ct = default);
    Task<GetMacroFaseDto?> GetByIdAsync(int progettoId, int id, CancellationToken ct = default);
    Task<IEnumerable<GetMacroFaseDto>> GetAllAsync(int progettoId, CancellationToken ct = default);
    Task<bool> UpdateAsync(UpdateMacroFaseDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int progettoId, int id, CancellationToken ct = default);

    // Tutte le macro fasi di un progetto con dentro le fasi
    Task<IEnumerable<MacroFaseWithFasiDto>> GetAllWithFasiAsync(int progettoId, CancellationToken ct = default);
}
