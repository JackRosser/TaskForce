using TaskForce.Dto;
using TaskForce.Dto.Progetto;

namespace TaskForce.Services
{
    public interface IProgettoService
    {
        Task<GetProgettoDto> CreateAsync(CreaProgettoDto dto, CancellationToken ct = default);
        Task<GetProgettoDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<GetProgettoDto>> GetAllAsync(CancellationToken ct = default);
        Task<bool> UpdateAsync(UpdateProgettoDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<GetProgettoWithFasiRequest>> GetAllWithInfoAsync(CancellationToken ct);
        Task<IEnumerable<PortfolioProjectOverviewDto>> GetPortfolioOverviewAsync(CancellationToken ct = default);
    }
}
